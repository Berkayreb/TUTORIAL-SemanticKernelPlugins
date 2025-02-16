using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace SemanticKernelPlugins.Server.Plugins
{
    public class HomePlugin
    {

        private readonly List<LightModel> lights = new()
   {
      new LightModel { Id = 1, Name = "Bathroom Lamp", IsOn = false },
      new LightModel { Id = 2, Name = "Kitchen Lamp", IsOn = false },
      new LightModel { Id = 3, Name = "Bedroom Lamp", IsOn = true }
   };

        [KernelFunction("get_lights")]
        [Description("Gets a list of lights and their current state")]
        [return: Description("An array of lights")]
        public Task<List<LightModel>> GetLightsAsync()
        {
            //GetFromIOTDevice();
            return Task.FromResult(lights);
        }

        [KernelFunction("change_state")]
        [Description("Changes the state of the light.If  Turn on or turn off lights state.This function changes the state when it toggles a specific light type on and off. ")]
        [return: Description("The update state of the light: will return if the light does not exist")]
        public Task<LightModel?> ChangeStateAsync(
            [Description("This is the ID of the light")]
        int id,
            [Description("True  if the light is on ; false if the light is off")]
        bool isOn)
        {
            var light = lights.FirstOrDefault(light => light.Id == id);

            if (light == null)
            {
                return null;
            }

            //ChangeStateOfIOTDevice();


            // Update the light with the new state
            light.IsOn = isOn;

            return Task.FromResult(light);
        }
    }

    public class LightModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_on")]
        public bool? IsOn { get; set; }

    }
}
