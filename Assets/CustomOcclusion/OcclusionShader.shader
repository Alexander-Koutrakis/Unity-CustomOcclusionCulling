Shader "Custom/OcclusionShader"
{
   
	Properties
    {
        _CameraRadius ("Camera Radius", Float) = 3.0
    }

    SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Overlay" "RenderPipeline" = "UniversalPipeline"}
		Pass
		{
			Name "CustomOcclusion Pass"

			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			ZTest Less
			CGPROGRAM
			#pragma vertex VSMain
			#pragma fragment PSMain
			#pragma target 5.0
			#pragma shader_feature DEBUG
			#include "UnityCG.cginc"

			struct OcclusionVertice
            {
                float3 position;
                uint isColisionOnly;
				uint index;
            };

			float _CameraRadius;
			RWStructuredBuffer<int> _Writer : register(u1);
			StructuredBuffer<OcclusionVertice> _Reader;
		
			int IsCameraInside(int boxIndex){
				float3 minimum = _Reader[boxIndex * 36].position;
				float3 maximum = _Reader[boxIndex * 36].position;

				float3 cameraPos = _WorldSpaceCameraPos;

				for (int i = 1; i < 36; ++i) {
					minimum = min(minimum, _Reader[boxIndex * 36 + i].position);
                    maximum = max(maximum, _Reader[boxIndex * 36 + i].position);
                }

				// Calculate the closest point on the bounding box to the camera position
				float3 closestPoint = clamp(cameraPos, minimum, maximum);

				// Calculate the distance between the camera position and the closest point
				float distanceToClosestPoint = distance(cameraPos, closestPoint);

				// Check if the distance is less than or equal to the radius of the camera's sphere (3 in this case)
				int cameraIsColliding = (distanceToClosestPoint <= _CameraRadius) ? 1 : 0;

				if(cameraIsColliding==1)
				{
					return cameraIsColliding;
				}

				int cameraIsInside = (cameraPos.x >= minimum.x && cameraPos.x <= maximum.x &&
                                 cameraPos.y >= minimum.y && cameraPos.y <= maximum.y &&
                                 cameraPos.z >= minimum.z && cameraPos.z <= maximum.z) ? 1 :0;

				return cameraIsInside;
			}
				

			float4 VSMain (float4 vertex : POSITION, out uint instance : TEXCOORD0 ,uint id : SV_VertexID) : SV_POSITION
			{				
				instance = _Reader[id].index;
				_Writer[instance]=IsCameraInside(instance);
				return mul (UNITY_MATRIX_VP, float4(_Reader[id].position, 1.0));
			}

			[earlydepthstencil]
			float4 PSMain (float4 vertex : SV_POSITION, uint instance : TEXCOORD0) : SV_TARGET
			{
				int isColisionOnly=_Reader[instance*36].isColisionOnly;
				int isCameraInside=_Writer[instance];
				
				if(isCameraInside!=1&&isColisionOnly!=1)
				{
					int isVisible= (dot(vertex, vertex) >= 0.0f) ? 1.0f : 0.0f;
				 	_Writer[instance]=isVisible;
				}
			
				#if DEBUG
					// If debugging, show the shader color
					return float4(0.0, 0.0, 1.0, 0.2);
                #else
					discard;
					return float4(0.0, 0.0, 0.0, 0.0); // Necessary return statement for when discard is not used.
                #endif
			}
			ENDCG
		}
	}
}
