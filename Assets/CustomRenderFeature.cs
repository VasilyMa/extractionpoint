using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class CustomRenderFeature : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        private string shaderPassName = "CustomPass"; // Имя pass вашего шейдера
        private Material material;

        public CustomRenderPass(Material material)
        {
            this.material = material;
            renderPassEvent = RenderPassEvent.AfterRenderingOpaques; // Или выберите подходящее для вашего случая
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cmd = CommandBufferPool.Get("CustomRenderPass");

            // Настройка DrawingSettings для вашего шейдера
            DrawingSettings drawingSettings = CreateDrawingSettings(new ShaderTagId(shaderPassName), ref renderingData, SortingCriteria.CommonOpaque);
            FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.opaque);

            // Выполнение отрисовки
            context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    public Material material; // Материал вашего шейдера
    CustomRenderPass customRenderPass;

    public override void Create()
    {
        if (material == null)
        {
            Debug.LogError("Убедитесь, что назначен материал в CustomRenderFeature!");
            return;
        }

        customRenderPass = new CustomRenderPass(material);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(customRenderPass);
    }
}
