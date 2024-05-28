using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using QuestSystem.Domain.Interfaces;
using QuestSystem.Domain.Models.Objectives;

public class ObjectiveJsonConverter : JsonConverter<IObjective>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(IObjective).IsAssignableFrom(typeToConvert);
    }

    public override IObjective Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            JsonElement jsonObject = doc.RootElement;
            
            IObjective objective = new AmountObjective(
                        jsonObject.GetProperty("Description").GetString()!,
                        jsonObject.GetProperty("Metric").GetString()!,
                        jsonObject.GetProperty("Goal").GetInt32());
            
            return objective;
        }
    }

    public override void Write(Utf8JsonWriter writer, IObjective value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("ObjectiveType", value.GetType().Name);
        writer.WriteString("Description", value.Description);
        writer.WriteString("Metric", value.Metric);

        if (value is AmountObjective amountObjective)
        {
            writer.WriteNumber("Goal", amountObjective.Goal);
        }
        
        writer.WriteEndObject();
    }
}
