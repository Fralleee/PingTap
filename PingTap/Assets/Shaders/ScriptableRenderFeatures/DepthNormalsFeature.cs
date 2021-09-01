using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR;

public class DepthNormalsFeature : ScriptableRendererFeature
{
  class DepthNormalsPass : ScriptableRenderPass
  {
    int kDepthBufferBits = 32;
    private RenderTargetHandle DepthAttachmentHandle { get; set; }
    internal RenderTextureDescriptor Descriptor { get; private set; }

    private Material depthNormalsMaterial = null;
    private FilteringSettings mFilteringSettings;
    string mProfilerTag = "DepthNormals Prepass";
    ShaderTagId mShaderTagId = new ShaderTagId("DepthOnly");

    public DepthNormalsPass(RenderQueueRange renderQueueRange, LayerMask layerMask, Material material)
    {
      mFilteringSettings = new FilteringSettings(renderQueueRange, layerMask);
      depthNormalsMaterial = material;
    }

    public void Setup(RenderTextureDescriptor baseDescriptor, RenderTargetHandle depthAttachmentHandle)
    {
      this.DepthAttachmentHandle = depthAttachmentHandle;
      baseDescriptor.colorFormat = RenderTextureFormat.ARGB32;
      baseDescriptor.depthBufferBits = kDepthBufferBits;
      Descriptor = baseDescriptor;
    }

    // This method is called before executing the render pass.
    // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
    // When empty this render pass will render to the active camera render target.
    // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
    // The render pipeline will ensure target setup and clearing happens in an performance manner.
    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
      cmd.GetTemporaryRT(DepthAttachmentHandle.id, Descriptor, FilterMode.Point);
      ConfigureTarget(DepthAttachmentHandle.Identifier());
      ConfigureClear(ClearFlag.All, Color.black);
    }

    // Here you can implement the rendering logic.
    // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
    // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
    // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
      CommandBuffer cmd = CommandBufferPool.Get(mProfilerTag);

      using (new ProfilingScope(cmd, profilingSampler))
      {
        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();

        var sortFlags = renderingData.cameraData.defaultOpaqueSortFlags;
        var drawSettings = CreateDrawingSettings(mShaderTagId, ref renderingData, sortFlags);
        drawSettings.perObjectData = PerObjectData.None;


        ref CameraData cameraData = ref renderingData.cameraData;
        Camera camera = cameraData.camera;
        if (XRSettings.enabled)
          context.StartMultiEye(camera);


        drawSettings.overrideMaterial = depthNormalsMaterial;


        context.DrawRenderers(renderingData.cullResults, ref drawSettings,
            ref mFilteringSettings);

        cmd.SetGlobalTexture("_CameraDepthNormalsTexture", DepthAttachmentHandle.id);
      }

      context.ExecuteCommandBuffer(cmd);
      CommandBufferPool.Release(cmd);
    }

    /// Cleanup any allocated resources that were created during the execution of this render pass.
    public override void FrameCleanup(CommandBuffer cmd)
    {
      if (DepthAttachmentHandle != RenderTargetHandle.CameraTarget)
      {
        cmd.ReleaseTemporaryRT(DepthAttachmentHandle.id);
        DepthAttachmentHandle = RenderTargetHandle.CameraTarget;
      }
    }
  }

  DepthNormalsPass depthNormalsPass;
  RenderTargetHandle depthNormalsTexture;
  Material depthNormalsMaterial;

  public override void Create()
  {
    depthNormalsMaterial = CoreUtils.CreateEngineMaterial("Hidden/Internal-DepthNormalsTexture");
    depthNormalsPass = new DepthNormalsPass(RenderQueueRange.opaque, -1, depthNormalsMaterial);
    depthNormalsPass.renderPassEvent = RenderPassEvent.AfterRenderingPrePasses;
    depthNormalsTexture.Init("_CameraDepthNormalsTexture");
  }

  // Here you can inject one or multiple render passes in the renderer.
  // This method is called when setting up the renderer once per-camera.
  public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
  {
    depthNormalsPass.Setup(renderingData.cameraData.cameraTargetDescriptor, depthNormalsTexture);
    renderer.EnqueuePass(depthNormalsPass);
  }
}

