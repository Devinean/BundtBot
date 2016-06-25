﻿using DiscordSharp;
using DiscordSharp.Events;
using DiscordSharp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DiscordSharp_Starter.BundtBot {
    class MessageReceivedProcessor {

        public void ProcessMessage(DiscordClient client, SoundBoard soundBoard, DiscordMessageEventArgs eventArgs) {
            if (eventArgs.MessageText == "!admin") {
                var admin = eventArgs.Author.Roles.Find(x => x.Name.Contains("Administrator"));
                string msg;
                if (admin != null) {
                    msg = "Yes, you are! :D";
                } else {
                    msg = "No, you aren't :c";
                }
                eventArgs.Channel.SendMessage(msg);
            }
            if (eventArgs.MessageText == "!mod") {
                bool ismod = false;
                List<DiscordRole> roles = eventArgs.Author.Roles;
                foreach (DiscordRole role in roles) {
                    if (role.Name.Contains("mod")) {
                        ismod = true;
                    }
                }
                if (ismod) {
                    eventArgs.Channel.SendMessage("Yes, you are! :D");
                } else {
                    eventArgs.Channel.SendMessage("No, you aren't D:");
                }
            }
            if (eventArgs.MessageText == "!help") {
                eventArgs.Channel.SendMessage("!owsb <character name> <phrase>");
                eventArgs.Channel.SendMessage("created by @AdenFlorian");
                eventArgs.Channel.SendMessage("https://github.com/AdenFlorian/DiscordSharp_Starter");
                eventArgs.Channel.SendMessage("https://trello.com/b/VKqUgzwV/bundtbot#");
            }
            if (eventArgs.MessageText == "!cat") {
                Random rand = new Random();
                if (rand.NextDouble() >= 0.5) {
                    string s;
                    using (WebClient webclient = new WebClient()) {
                        s = webclient.DownloadString("http://random.cat/meow");
                        int pFrom = s.IndexOf("\\/i\\/") + "\\/i\\/".Length;
                        int pTo = s.LastIndexOf("\"}");
                        string cat = s.Substring(pFrom, pTo - pFrom);
                        Console.WriteLine("http://random.cat/i/" + cat);
                        eventArgs.Channel.SendMessage("I found a cat\nhttp://random.cat/i/" + cat);
                    }
                } else {
                    Dog(eventArgs, "how about a dog instead");
                }
            }
            if (eventArgs.MessageText == "!dog") {
                Dog(eventArgs, "i found a dog");
            }

            #region SoundBoard

            if (eventArgs.MessageText == "!stop") {
                if (client.GetVoiceClient() == null) {
                    eventArgs.Channel.SendMessage("stop what? (client.GetVoiceClient() returned null)");
                } else if (client.GetVoiceClient().Connected == false) {
                    eventArgs.Channel.SendMessage("stop what? (client.GetVoiceClient().Connected == false)");
                } else {
                    eventArgs.Channel.SendMessage("okay... :disappointed_relieved:");
                    soundBoard.stop = true;
                }
            }

            if (eventArgs.MessageText.StartsWith("!owsb ") ||
                eventArgs.MessageText.StartsWith("!sb")) {
                SoundBoardArgs soundBoardArgs = null;
                try {
                    soundBoardArgs = new SoundBoardArgs(eventArgs.MessageText);
                } catch (Exception e) {
                    eventArgs.Channel.SendMessage("you're doing it wrong");
                    eventArgs.Channel.SendMessage(e.Message);
                }

                if (soundBoardArgs == null) {
                    eventArgs.Channel.SendMessage("you're doing it wrong (or something broke)");
                    return;
                }

                soundBoard.Process(eventArgs, soundBoardArgs);
            }
            #endregion
        }

        private static void Dog(DiscordSharp.Events.DiscordMessageEventArgs eventArgs, string message) {
            try {
                string s;
                using (WebClient webclient = new MyWebClient()) {
                    s = webclient.DownloadString("http://random.dog/woof");
                    string dog = s;
                    Console.WriteLine("http://random.dog/" + dog);
                    eventArgs.Channel.SendMessage(message + "\nhttp://random.dog/" + dog);
                }
            } catch (Exception) {
                eventArgs.Channel.SendMessage("there are no dogs here, who let them out (random.dog is down :dog: :interrobang:)");
            }

        }
    }
}
