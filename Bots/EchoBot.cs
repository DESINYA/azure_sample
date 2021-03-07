// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Linq;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class EchoBot : ActivityHandler
    {
        private static List<Student> student_list = new List<Student>();
        private static List<Prefecture> prefecture_list = new List<Prefecture>();

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var pref = from prefecture in prefecture_list where prefecture.prefecture_name.IndexOf(turnContext.Activity.Text) > -1 select prefecture;
            string replyText = "";
            if (pref.Count() > 0)
            {
                replyText = pref.First().prefecture_name;
            }
            else
            {
                replyText = "見つかりませんでした。";
            }
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            string welcomeText = "";
            using (LocalAppDb context = new LocalAppDb())
            {
                try
                {
                    student_list = context.students.ToList();
                    prefecture_list = context.prefecture.ToList();
                }
                catch (Exception e)
                {
                    welcomeText = "Connection Error\n"+e.Message;
                }
            }
            if (welcomeText.Length == 0)
            {
                welcomeText = "Hello and ウェルコネ！";
            }

            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
