using Microsoft.PowerPlatform.PowerAutomate.Desktop.Actions.SDK;
using Microsoft.PowerPlatform.PowerAutomate.Desktop.Actions.SDK.Attributes;
using System;
using Azure;
using Azure.AI.OpenAI;

namespace Modules.AIPADAction
{
    [Action(Id = "Action1", Order = 1, FriendlyName = "AzureOpenAI Action", Description = "AzureOpenAI action for Intelligent processing")]
    [Throws("ActionError")] // TODO: change error name (or delete if not needed)
    public class Action1 : ActionBase
    {
        #region Properties

        [InputArgument(FriendlyName = "Prompt", Description = "Please provide your prompt")]
        public string InputArgument1 { get; set; }

        [InputArgument(FriendlyName = "AzureOpenAI Key", Description = "Please provide your OpenAI key")]
        public string InputArgument2 { get; set; }

        [OutputArgument(FriendlyName = "Intelligent Output", Description = "AI response")]
        public string OutputArgument1 { get; set; }

        #endregion

        #region Methods Overrides

        public override void Execute(ActionContext context)
        {
            try
            {
                OpenAIClient client = new OpenAIClient(
                new Uri("https://<youropenAIURL>.openai.azure.com/"),
                new AzureKeyCredential(InputArgument2));

                Response<ChatCompletions> responseWithoutStream = client.GetChatCompletions("<Yourmodelname>",
                new ChatCompletionsOptions()
                {
                    Messages =
                  {
                     new ChatMessage(ChatRole.System, InputArgument1),

                  },
                    Temperature = (float)0.7,
                    MaxTokens = 800,


                    NucleusSamplingFactor = (float)0.95,
                    FrequencyPenalty = 0,
                    PresencePenalty = 0,
                });

                ChatCompletions response = responseWithoutStream.Value;
                OutputArgument1 = response.Choices[0].Message.Content;
                //TODO: add action execution code here
            }
            catch (Exception e)
            {
                if (e is ActionException) throw;

                throw new ActionException("ActionError", e.Message, e.InnerException);
            }

            // TODO: set values to Output Arguments here
        }

        #endregion
    }
}
