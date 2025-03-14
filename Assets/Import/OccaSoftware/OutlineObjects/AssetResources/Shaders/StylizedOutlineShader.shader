Shader "Universal Render Pipeline/OccaSoftware/Outline Objects"
{
    Properties
    {
        [HDR]_OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineThickness ("Outline Thickness", Float) = 0.1
        _CompleteFalloffDistance ("Complete Falloff Distance", Float) = 30.0
        _NoiseTexture ("Noise Texture", 2D) = "white" {} 
        _NoiseFrequency ("Noise Frequency", Float) = 5.0
        _NoiseFramerate ("Noise Framerate", Float) = 12.0
        
        [Toggle(_USE_VERTEX_COLOR_ENABLED)] _USE_VERTEX_COLOR_ENABLED ("Use Vertex Color (R) for Outline Thickness?", Float) = 0
        [Toggle(_ATTENUATE_BY_DISTANCE_ENABLED)] _ATTENUATE_BY_DISTANCE_ENABLED("Attenuate Outline Thickness by Camera Distance?", Float) = 0
        [Toggle(_RANDOM_OFFSETS_ENABLED)] _RANDOM_OFFSETS_ENABLED("Randomly offset the sample position", Float) = 0
        [Toggle(_USE_SMOOTHED_NORMALS_ENABLED)] _USE_SMOOTHED_NORMALS_ENABLED("Use Smoothed Normals (UV3)", Float) = 0
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" "LightMode" = "SRPDefaultUnlit"}
        ZWrite On
        ZTest LEqual
        Cull Front
        
        Pass
        {
            Name "DrawOutlines"
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            
            #pragma multi_compile_instancing
            
            #include "OutlineObjects.hlsl"
            ENDHLSL
        }
        
        Pass
        {
            Name "DepthOnly"
            Tags {"LightMode" = "DepthOnly"}
            
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment FragmentDepthOnly
            
            #pragma multi_compile_instancing
            
            #include "OutlineObjects.hlsl"
            ENDHLSL
        }
        
        Pass
        {
            Name "DepthNormalsOnly"
            Tags {"LightMode" = "DepthNormalsOnly"}
            
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment FragmentDepthNormalsOnly
            
            #pragma multi_compile_instancing
            
            #include "OutlineObjects.hlsl"
            ENDHLSL
        }
    }
    
    CustomEditor "OccaSoftware.OutlineObjects.Editor.OutlineObjectsShaderGUI"
}
