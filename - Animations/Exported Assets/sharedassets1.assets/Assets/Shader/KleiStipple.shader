Shader "Klei/Stipple" {
	Properties {
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_Color ("Color", Vector) = (1,1,1,1)
	}
	SubShader {
		LOD 100
		Tags { "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent+500" "RenderType" = "Transparent" }
		Pass {
			LOD 100
			Tags { "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent+500" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			GpuProgramID 52162
			Program "vp" {
				SubProgram "d3d11 " {
					"!!vs_4_0
					//
					// Generated by Microsoft (R) D3D Shader Disassembler
					//
					//
					// Input signature:
					//
					// Name                 Index   Mask Register SysValue  Format   Used
					// -------------------- ----- ------ -------- -------- ------- ------
					// POSITION                 0   xyzw        0     NONE   float   xyzw
					// TEXCOORD                 0   xy          1     NONE   float   xy  
					// COLOR                    0   xyzw        2     NONE   float   xyzw
					//
					//
					// Output signature:
					//
					// Name                 Index   Mask Register SysValue  Format   Used
					// -------------------- ----- ------ -------- -------- ------- ------
					// SV_POSITION              0   xyzw        0      POS   float   xyzw
					// TEXCOORD                 0   xy          1     NONE   float   xy  
					// TEXCOORD                 1   xyz         2     NONE   float   xyz 
					// TEXCOORD                 2   xyzw        3     NONE   float   xyzw
					//
					vs_4_0
					dcl_constantbuffer CB0[9], immediateIndexed
					dcl_constantbuffer CB1[4], immediateIndexed
					dcl_constantbuffer CB2[21], immediateIndexed
					dcl_input v0.xyzw
					dcl_input v1.xy
					dcl_input v2.xyzw
					dcl_output_siv o0.xyzw, position
					dcl_output o1.xy
					dcl_output o2.xyz
					dcl_output o3.xyzw
					dcl_temps 2
					mul r0.xyzw, v0.yyyy, cb1[1].xyzw
					mad r0.xyzw, cb1[0].xyzw, v0.xxxx, r0.xyzw
					mad r0.xyzw, cb1[2].xyzw, v0.zzzz, r0.xyzw
					add r1.xyzw, r0.xyzw, cb1[3].xyzw
					mad o2.xyz, cb1[3].xyzx, v0.wwww, r0.xyzx
					mul r0.xyzw, r1.yyyy, cb2[18].xyzw
					mad r0.xyzw, cb2[17].xyzw, r1.xxxx, r0.xyzw
					mad r0.xyzw, cb2[19].xyzw, r1.zzzz, r0.xyzw
					mad o0.xyzw, cb2[20].xyzw, r1.wwww, r0.xyzw
					mov o1.xy, v1.xyxx
					mul o3.xyz, v2.xyzx, cb0[8].xyzx
					mov o3.w, v2.w
					ret 
					// Approximately 0 instruction slots used"
				}
			}
			Program "fp" {
				SubProgram "d3d11 " {
					"!!ps_4_0
					//
					// Generated by Microsoft (R) D3D Shader Disassembler
					//
					//
					// Input signature:
					//
					// Name                 Index   Mask Register SysValue  Format   Used
					// -------------------- ----- ------ -------- -------- ------- ------
					// SV_POSITION              0   xyzw        0      POS   float       
					// TEXCOORD                 0   xy          1     NONE   float   xy  
					// TEXCOORD                 1   xyz         2     NONE   float   xy  
					// TEXCOORD                 2   xyzw        3     NONE   float   xyz 
					//
					//
					// Output signature:
					//
					// Name                 Index   Mask Register SysValue  Format   Used
					// -------------------- ----- ------ -------- -------- ------- ------
					// SV_Target                0   xyzw        0   TARGET   float   xyzw
					// SV_Target                1   xyzw        1   TARGET   float   xyzw
					// SV_Target                2   xyzw        2   TARGET   float   xyzw
					//
					ps_4_0
					dcl_constantbuffer CB0[5], immediateIndexed
					dcl_sampler s0, mode_default
					dcl_sampler s1, mode_default
					dcl_resource_texture2d (float,float,float,float) t0
					dcl_resource_texture2d (float,float,float,float) t1
					dcl_input_ps linear v1.xy
					dcl_input_ps linear v2.xy
					dcl_input_ps linear v3.xyz
					dcl_output o0.xyzw
					dcl_output o1.xyzw
					dcl_output o2.xyzw
					dcl_temps 2
					mad r0.xy, v2.xyxx, cb0[3].xyxx, cb0[3].zwzz
					sample r0.xyzw, r0.xyxx, t1.xyzw, s0
					sample r1.xyzw, v1.xyxx, t0.xyzw, s1
					add r0.x, -r1.w, l(1.000000)
					add r0.x, -r0.x, r0.w
					lt r0.y, r1.w, l(1.000000)
					mul o0.xyz, r1.xyzx, v3.xyzx
					movc r0.x, r0.y, r0.x, l(1.000000)
					ge r0.y, cb0[4].w, l(1.000000)
					movc o0.w, r0.y, r0.x, l(1.000000)
					mov o1.xyzw, l(1.000000,0,0,1.000000)
					mov o2.xyzw, l(0,0,0,0)
					ret 
					// Approximately 0 instruction slots used"
				}
			}
		}
	}
}