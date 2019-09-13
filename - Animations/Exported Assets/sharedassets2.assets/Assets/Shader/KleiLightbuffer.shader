Shader "Klei/Lightbuffer" {
	Properties {
		_MainTex ("MainTex", 2D) = "white" {}
		_PropertyWorldLight ("WorldLight", 2D) = "white" {}
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha One, SrcAlpha One
			ColorMask RGB -1
			ZWrite Off
			Cull Off
			GpuProgramID 48750
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
					//
					//
					// Output signature:
					//
					// Name                 Index   Mask Register SysValue  Format   Used
					// -------------------- ----- ------ -------- -------- ------- ------
					// SV_POSITION              0   xyzw        0      POS   float   xyzw
					// TEXCOORD                 0   xyzw        1     NONE   float   xyzw
					//
					vs_4_0
					dcl_constantbuffer CB0[11], immediateIndexed
					dcl_constantbuffer CB1[4], immediateIndexed
					dcl_constantbuffer CB2[21], immediateIndexed
					dcl_input v0.xyzw
					dcl_input v1.xy
					dcl_output_siv o0.xyzw, position
					dcl_output o1.xyzw
					dcl_temps 2
					mul r0.xyzw, v0.yyyy, cb1[1].xyzw
					mad r0.xyzw, cb1[0].xyzw, v0.xxxx, r0.xyzw
					mad r0.xyzw, cb1[2].xyzw, v0.zzzz, r0.xyzw
					add r1.xyzw, r0.xyzw, cb1[3].xyzw
					mad r0.xy, cb1[3].xyxx, v0.wwww, r0.xyxx
					mul o1.zw, r0.xxxy, cb0[6].zzzw
					mul r0.xyzw, r1.yyyy, cb2[18].xyzw
					mad r0.xyzw, cb2[17].xyzw, r1.xxxx, r0.xyzw
					mad r0.xyzw, cb2[19].xyzw, r1.zzzz, r0.xyzw
					mad o0.xyzw, cb2[20].xyzw, r1.wwww, r0.xyzw
					mad o1.xy, v1.xyxx, cb0[10].xyxx, cb0[10].zwzz
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
					// TEXCOORD                 0   xyzw        1     NONE   float   xyzw
					//
					//
					// Output signature:
					//
					// Name                 Index   Mask Register SysValue  Format   Used
					// -------------------- ----- ------ -------- -------- ------- ------
					// SV_Target                0   xyzw        0   TARGET   float   xyzw
					//
					ps_4_0
					dcl_constantbuffer CB0[12], immediateIndexed
					dcl_sampler s0, mode_default
					dcl_sampler s1, mode_default
					dcl_sampler s2, mode_default
					dcl_resource_texture2d (float,float,float,float) t0
					dcl_resource_texture2d (float,float,float,float) t1
					dcl_resource_texture2d (float,float,float,float) t2
					dcl_input_ps linear v1.xyzw
					dcl_output o0.xyzw
					dcl_temps 2
					sample r0.xyzw, v1.zwzz, t2.xyzw, s2
					ge r0.x, l(0.000000), r0.w
					discard_nz r0.x
					sample r1.xyzw, v1.zwzz, t0.xyzw, s0
					mad_sat r0.x, -r1.x, l(2.000000), l(1.000000)
					sample r1.xyzw, v1.xyxx, t1.xyzw, s1
					mul r0.y, r1.w, cb0[11].x
					mul o0.xyz, r0.xxxx, r0.yyyy
					mov o0.w, r0.w
					ret 
					// Approximately 0 instruction slots used"
				}
			}
		}
	}
}