Shader "Shapes/Polyline 2D Opaque" {
	Properties {
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("Z Test", int) = 4
		_ZOffsetFactor ("Z Offset Factor", Float ) = 0
		_ZOffsetUnits ("Z Offset Units", int ) = 0
	}
	SubShader {
		Tags {
			"RenderPipeline" = "UniversalPipeline"
			"IgnoreProjector" = "True"
			"Queue" = "AlphaTest"
			"RenderType" = "TransparentCutout"
			"DisableBatching" = "True"
		}
		Pass {
			Name "Pass"
			Tags { "LightMode" = "UniversalForward" }
			Cull Off
			ZTest [_ZTest]
			Offset [_ZOffsetFactor], [_ZOffsetUnits]
			AlphaToMask On
			HLSLPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_instancing
				#pragma prefer_hlslcc gles
				#pragma exclude_renderers d3d11_9x
				#pragma target 2.0
				#pragma multi_compile __ IS_JOIN_MESH
				#pragma multi_compile __ JOIN_MITER JOIN_ROUND JOIN_BEVEL
				#define OPAQUE
				#include "../../Core/Polyline 2D Core.cginc"
			ENDHLSL
		}
		Pass {
			Name "DepthOnly"
			Tags { "LightMode" = "DepthOnly" }
			Cull Off
			HLSLPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_instancing
				#pragma prefer_hlslcc gles
				#pragma exclude_renderers d3d11_9x
				#pragma target 2.0
				#pragma multi_compile __ IS_JOIN_MESH
				#pragma multi_compile __ JOIN_MITER JOIN_ROUND JOIN_BEVEL
				#define OPAQUE
				#include "../../Core/Polyline 2D Core.cginc"
			ENDHLSL
		}
		Pass {
			Name "Picking"
			Tags { "LightMode" = "Picking" }
			Cull Off
			HLSLPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_instancing
				#pragma prefer_hlslcc gles
				#pragma exclude_renderers d3d11_9x
				#pragma target 2.0
				#pragma multi_compile __ IS_JOIN_MESH
				#pragma multi_compile __ JOIN_MITER JOIN_ROUND JOIN_BEVEL
				#define OPAQUE
				#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
				#define SCENE_VIEW_PICKING
				#include "../../Core/Polyline 2D Core.cginc"
			ENDHLSL
		}
		Pass {
			Name "Selection"
			Tags { "LightMode" = "SceneSelectionPass" }
			Cull Off
			HLSLPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_instancing
				#pragma prefer_hlslcc gles
				#pragma exclude_renderers d3d11_9x
				#pragma target 2.0
				#pragma multi_compile __ IS_JOIN_MESH
				#pragma multi_compile __ JOIN_MITER JOIN_ROUND JOIN_BEVEL
				#define OPAQUE
				#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
				#define SCENE_VIEW_OUTLINE_MASK
				#include "../../Core/Polyline 2D Core.cginc"
			ENDHLSL
		}
	}
}