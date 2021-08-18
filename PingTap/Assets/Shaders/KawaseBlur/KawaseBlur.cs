using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class KawaseBlur : ScriptableRendererFeature
{
  [System.Serializable]
  public class KawaseBlurSettings
  {
    public RenderPassEvent RenderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    public Material BlurMaterial = null;

    [Range(2, 15)]
    public int BlurPasses = 1;

    [Range(1, 4)]
    public int Downsample = 1;
    public bool CopyToFramebuffer;
    public string TargetName = "_blurTexture";
  }

  public KawaseBlurSettings Settings = new KawaseBlurSettings();

  class CustomRenderPass : ScriptableRenderPass
  {
    public Material BlurMaterial;
    public int Passes;
    public int Downsample;
    public bool CopyToFramebuffer;
    public string TargetName;
    string profilerTag;

    int tmpId1;
    int tmpId2;

    RenderTargetIdentifier tmpRt1;
    RenderTargetIdentifier tmpRt2;

    private RenderTargetIdentifier Source { get; set; }

    private void Setup(RenderTargetIdentifier source)
    {
      Source = source;
    }

    public CustomRenderPass(string profilerTag)
    {
      this.profilerTag = profilerTag;
    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
      int width = cameraTextureDescriptor.width / Downsample;
      int height = cameraTextureDescriptor.height / Downsample;

      tmpId1 = Shader.PropertyToID("tmpBlurRT1");
      tmpId2 = Shader.PropertyToID("tmpBlurRT2");
      cmd.GetTemporaryRT(tmpId1, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
      cmd.GetTemporaryRT(tmpId2, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);

      tmpRt1 = new RenderTargetIdentifier(tmpId1);
      tmpRt2 = new RenderTargetIdentifier(tmpId2);

      ConfigureTarget(tmpRt1);
      ConfigureTarget(tmpRt2);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
      CommandBuffer cmd = CommandBufferPool.Get(profilerTag);

      RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
      opaqueDesc.depthBufferBits = 0;

      // first pass
      // cmd.GetTemporaryRT(tmpId1, opaqueDesc, FilterMode.Bilinear);
      cmd.SetGlobalFloat("_offset", 1.5f);
      cmd.Blit(Source, tmpRt1, BlurMaterial);

      for (int i = 1; i < Passes - 1; i++)
      {
        cmd.SetGlobalFloat("_offset", 0.5f + i);
        cmd.Blit(tmpRt1, tmpRt2, BlurMaterial);

        // pingpong
        RenderTargetIdentifier rttmp = tmpRt1;
        tmpRt1 = tmpRt2;
        tmpRt2 = rttmp;
      }

      // final pass
      cmd.SetGlobalFloat("_offset", 0.5f + Passes - 1f);
      if (CopyToFramebuffer)
      {
        cmd.Blit(tmpRt1, Source, BlurMaterial);
      }
      else
      {
        cmd.Blit(tmpRt1, tmpRt2, BlurMaterial);
        cmd.SetGlobalTexture(TargetName, tmpRt2);
      }

      context.ExecuteCommandBuffer(cmd);
      cmd.Clear();

      CommandBufferPool.Release(cmd);
    }

    public override void FrameCleanup(CommandBuffer cmd)
    {
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
      ScriptableRenderer renderer = renderingData.cameraData.renderer;
      RenderTargetIdentifier src = renderer.cameraColorTarget;
      Setup(src);
    }
  }

  CustomRenderPass scriptablePass;

  public override void Create()
  {
    scriptablePass = new CustomRenderPass("KawaseBlur")
    {
      BlurMaterial = Settings.BlurMaterial,
      Passes = Settings.BlurPasses,
      Downsample = Settings.Downsample,
      CopyToFramebuffer = Settings.CopyToFramebuffer,
      TargetName = Settings.TargetName,
      renderPassEvent = Settings.RenderPassEvent
    };

  }

  public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
  {
    renderer.EnqueuePass(scriptablePass);
  }
}
