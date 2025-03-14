Shader "SimpleURPToonLitExample(With Outline)"
{
    Properties
    {
        
        [Header(Base Color)]
        [MainTexture]_BaseMap("Base Map", 2D) = "white" {}
        [HDR][MainColor]_BaseColor("Base Color", Color) = (1,1,1,1)

        [Header(Direct Light)]
        _DirectLightMultiplier("Brightness", Range(0,1)) = 1
        _CelShadeMidPoint("MidPoint", Range(-1,1)) = -0.5
        _CelShadeSoftness("Softness", Range(0,1)) = 0.05
        _MainLightIgnoreCelShade("Remove Shadow", Range(0,1)) = 0
        
        [Header(Outline)]
        _OutlineWidth("Width", Range(0,10)) = 1
        _OutlineColor("Color", Color) = (0.5,0.5,0.5,1)
        
        [Header(Outline ZOffset)]
        _OutlineZOffset("ZOffset (View Space)", Range(0,1)) = 0.0001
        [NoScaleOffset]_OutlineZOffsetMaskTex("    Mask (black is apply ZOffset)", 2D) = "black" {}
        _OutlineZOffsetMaskRemapStart("    RemapStart", Range(0,1)) = 0
        _OutlineZOffsetMaskRemapEnd("    RemapEnd", Range(0,1)) = 1

    }
    SubShader
    {       
        Tags 
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
            "IgnoreProjector" = "True"
            "UniversalMaterialType" = "ComplexLit"
            "Queue"="Geometry"
        }
        LOD 100
        
        HLSLINCLUDE

        #pragma shader_feature_local_fragment _UseAlphaClipping

        ENDHLSL

         Pass
        {               
            Name "ForwardLit"
            Tags
            {
                // "LightMode" matches the "ShaderPassName" set in UniversalRenderPipeline.cs. 
                // SRPDefaultUnlit and passes with no LightMode tag are also rendered by URP

                // "LightMode" tag must be "UniversalForward" in order to render lit objects in URP.
                "LightMode" = "UniversalForwardOnly"
            }

            // -------------------------------------
            // Render State Commands
            // - explicit render state to avoid confusion
            // - you can expose these render state to material inspector if needed (see URP's Lit.shader)
            Blend One Zero
            ZWrite On
            Cull Off
            ZTest LEqual

            HLSLPROGRAM
            #pragma target 2.0

            // -------------------------------------
            // Shader Stages
            #pragma vertex VertexShaderWork
            #pragma fragment ShadeFinalColor

            // -------------------------------------
            // Material Keywords
            // (all shader_feature that we needed were extracted to a shared SubShader level HLSL block already)
            
            // -------------------------------------
            // Universal Pipeline keywords
            // You can always copy this section from URP's ComplexLit.shader
            // When doing custom shaders you most often want to copy and paste these #pragma multi_compile
            // These multi_compile variants are stripped from the build depending on:
            // 1) Settings in the URP Asset assigned in the GraphicsSettings at build time
            // e.g If you disabled AdditionalLights in all the URP assets then all _ADDITIONA_LIGHTS variants
            // will be stripped from build
            // 2) Invalid combinations are stripped.
            #pragma multi_compile _ _FORWARD_PLUS
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"

            // -------------------------------------
            // Unity defined keywords

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"

            //--------------------------------------
            // Defines
            // - because this pass is just a ForwardLitOnly pass, no need any special #define
            // (no special #define)

            // -------------------------------------
            // Includes
            // - all shader logic written inside this .hlsl, remember to write all #define BEFORE writing #include
            #include "SimpleURPToonLitOutlineExample_Shared.hlsl"

            ENDHLSL
        }
        
        // [#1 Pass - Outline]
        // Same as the above "ForwardLit" pass, but: 
        // - vertex position are pushed out a bit base on normal direction
        // - also color is tinted by outline color
        // - Cull Front instead of Cull Off because Cull Front is a must for any 2 pass outline method
        Pass 
        {
            Name "Outline"
            Tags 
            {
                // IMPORTANT: don't write this line for any custom pass(e.g. outline pass)! 
                // else this outline pass(custom pass) will not be rendered by URP!
                "LightMode" = "CustomPass" 

                // [Important CPU performance note]
                // If you need to add a custom pass to your shader (e.g. outline pass, planar shadow pass, Xray overlay pass when blocked....),
                // follow these steps:
                // (1) Add a new Pass{} to your shader
                // (2) Write "LightMode" = "YourCustomPassTag" inside new Pass's Tags{}
                // (3) Add a new custom RendererFeature(C#) to your renderer,
                // (4) write cmd.DrawRenderers() with ShaderPassName = "YourCustomPassTag"
                // (5) if done correctly, URP will render your new Pass{} for your shader, in a SRP-batching friendly way (usually in 1 big SRP batch)

                // For tutorial purpose, current everything is just shader files without any C#, so this Outline pass is actually NOT SRP-batching friendly.
                // If you are working on a project with lots of characters, make sure you use the above method to make Outline pass SRP-batching friendly!
            }

            // -------------------------------------
            // Render State Commands
            // - Cull Front is a must for extra pass outline method
            Blend One Zero
            ZWrite On
            Cull Front
            ZTest LEqual

            HLSLPROGRAM
            #pragma target 2.0
            
            // -------------------------------------
            // Shader Stages
            #pragma vertex VertexShaderWork
            #pragma fragment ShadeFinalColor

            // -------------------------------------
            // Material Keywords
            // (all shader_feature that we needed were extracted to a shared SubShader level HLSL block already)
            
            // -------------------------------------
            // Universal Pipeline keywords
            #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
            #pragma multi_compile _ _FORWARD_PLUS
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"


            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fog
            #pragma multi_compile_fragment _ DEBUG_DISPLAY

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"

            #define ToonShaderIsOutline

            #include "SimpleURPToonLitOutlineExample_Shared.hlsl"

            ENDHLSL
        }

 
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
    
}