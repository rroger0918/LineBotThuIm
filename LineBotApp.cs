using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace LineBotThuIm
{
    public class LineBotApp : WebhookApplication
    {
        private readonly LineMessagingClient _messagingClient;

        public LineBotApp(LineMessagingClient lineMessagingClient)
        {
            _messagingClient = lineMessagingClient;
        }
        public class JsonProvider
        {
            private JsonSerializerOptions serializeOption = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            private static JsonSerializerOptions deserializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            public string Serialize<T>(T obj)
            {
                return JsonSerializer.Serialize(obj, serializeOption);
            }

            public T Deserialize<T>(string str)
            {
                return JsonSerializer.Deserialize<T>(str, deserializeOptions);
            }
        }
        private readonly string replyMessageUri = "https://api.line.me/v2/bot/message/reply";
        private static HttpClient client = new HttpClient();
        private readonly JsonProvider _jsonProvider = new JsonProvider();

        /// <summary>
        /// 接收到回覆請求時，在將請求傳至 Line 前多一層處理(目前為預留)
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="requestBody"></param>
        public void ReplyMessageHandler<T>(ReplyMessageRequestDto<T> requestBody)
        {
            ReplyMessage(requestBody);
        }
        
        /// <summary>
        /// 將回覆訊息請求送到 Line
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        public async void ReplyMessage<T>(ReplyMessageRequestDto<T> request)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "d6LDOh0pCbl0oMEoJ9Y/Xitg1GYf7ZzTFQpFxf6Cn+qZSrSMYXDmEZp6bzAkwOmm/KhgXcZYf1K4NH3UGcK20syEZLbP1YyR97wBmABazofnfHXmDSbJW7/ublngiCr0eE8K9sCt/uh8wVbWiHR/rgdB04t89/1O/w1cDnyilFU="); //帶入 channel access token
            var json = _jsonProvider.Serialize(request);
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(replyMessageUri),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(requestMessage);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        protected override async Task OnMessageAsync(MessageEvent ev)
        {
            var result = null as List<ISendMessage>;
            dynamic result2 = new ReplyMessageRequestDto<BaseMessageModel>();
            var messageText = ((TextEventMessage)ev.Message).Text;
            ////頻道Id
            //var channelId = ev.Source.Id;
            ////使用者Id
            //var userId = ev.Source.UserId;

            switch (ev.Message)
            {
                //文字訊息
                case TextEventMessage textMessage:
                    {
                        //快速選單
                        var quickReply = new QuickReply
                        {
                            Items = new List<QuickReplyButtonObject>{
                              new QuickReplyButtonObject(
                                       new MessageTemplateAction("A.同學會資訊?", "同學會辦在哪"),
                                       imageUrl: "https://res.cloudinary.com/dtghn46kc/image/upload/v1676198006/A_kmofwz.png"),
                              new QuickReplyButtonObject(
                                       new MessageTemplateAction("B.快速填寫活動表單?", "快速填寫活動表單"),
                                       imageUrl: "https://res.cloudinary.com/dtghn46kc/image/upload/v1676198007/B_dsgjtf.png"),
                              new QuickReplyButtonObject(
                                       new MessageTemplateAction("C.上傳相簿?", "回憶相簿"),
                                       imageUrl: "https://res.cloudinary.com/dtghn46kc/image/upload/v1676198006/C_mcrrr1.png"),
                              new QuickReplyButtonObject(
                                       new MessageTemplateAction("D.昂哥經歷與偉業?", "可以讓我看看昂哥多屌嗎"),
                                       imageUrl: "https://res.cloudinary.com/dtghn46kc/image/upload/v1676198006/D_ptis1g.png"),
                             new QuickReplyButtonObject(
                                       new MessageTemplateAction("E.昂哥系列貼圖?", "我想買昂哥系列貼圖"),
                                       imageUrl: "https://res.cloudinary.com/dtghn46kc/image/upload/v1676198007/E_r8tq9m.png"),
                              new QuickReplyButtonObject(
                                       new MessageTemplateAction("F.茗爺家傳肉乾?", "我想買茗爺家傳肉乾"),
                                       imageUrl: "https://res.cloudinary.com/dtghn46kc/image/upload/v1676198007/F_argkt2.png"),
                              new QuickReplyButtonObject(
                                       new MessageTemplateAction("G.黑狗軒免運布朗尼?", "我想買黑狗軒免運布朗尼"),
                                       imageUrl: "https://res.cloudinary.com/dtghn46kc/image/upload/v1676198007/G_zkbqjk.jpg"),
                              new QuickReplyButtonObject(
                                       new MessageTemplateAction("H.集點卡?", "集點"),
                                       imageUrl: "https://res.cloudinary.com/dtghn46kc/image/upload/v1676198006/H_hbvyt1.png")
                            }
                        };

                        //快速選單內容
                        if (messageText == "同學會辦在哪")
                        {
                            result = new List<ISendMessage> { new LocationMessage("9/9 雲林 三好酒店", "364 苗栗縣苗栗市苗栗縣大湖鄉富興村七鄰八寮灣33-5號", (decimal)23.71793714066947, (decimal)120.53615235986366) };
                            result.Add(new TextMessage("https://thuform.azurewebsites.net/Home/Theme"));
                        }
                        else if (messageText == "快速填寫活動表單")
                        {
                            result = new List<ISendMessage> { new TextMessage("快速填寫活動表單 Go") };
                            result.Add(new TextMessage("https://thuform.azurewebsites.net/Home/Contact"));
                        }
                        else if (messageText == "回憶相簿")
                        {
                            result = new List<ISendMessage> { new TextMessage("回憶相簿 - 期待您共同參與") };
                            result.Add(new TextMessage("https://drive.google.com/drive/folders/16gy2qvT1nrPcEvASOot_wBvGOQTgrhA8?usp=sharing"));
                        }
                        else if (messageText == "可以讓我看看昂哥多屌嗎")
                        {
                            result = new List<ISendMessage> { new TextMessage("\U0001F409東海龍王昂哥，是現任高雄苗栗屏東花蓮及新北三重蘆洲地方大老、企業公安管理顧問師、海軍陸戰隊培訓師、職業演說家、並經營一間AI砂石場，每年吸引超過20億的資金投資。\n\n☠真槍實彈實戰經驗23年，商業經營15年。從陌生派系開發一路經營到社會上流，專精於地方營運、公安設計、組織經營與管理。\n\n☢服務過的客戶包含伊拉克、美國蛙人部隊、俄羅斯北極熊生擒小隊、亞馬遜食人魚活烤三吃小隊、越南科摩多龍生擒小隊、中國新冠肺炎生擒小隊、…等百大特殊單位。\n\n\U0001F479昂哥公司協助各級組織創建永續發展的地方經營策略，致力於訓練、建立、指導高績效團隊。他以其專業領域「戰鬥哲學」為基礎，設計出獨特的指導手法，以最簡單直白的方式，協助各行各業中埋伏的內賊及地方樁腳大幅提升資產，廣受好評。") };
                            result.Add(new ImageMessage("https://res.cloudinary.com/dtghn46kc/image/upload/v1676197682/%E6%9D%B1%E6%B5%B7%E9%BE%8D%E7%8E%8B%E6%98%82%E5%93%A5_yg3xqv.jpg", "https://res.cloudinary.com/dtghn46kc/image/upload/v1676197682/%E6%9D%B1%E6%B5%B7%E9%BE%8D%E7%8E%8B%E6%98%82%E5%93%A5_yg3xqv.jpg"));
                        }
                        else if (messageText == "我想買昂哥系列貼圖")
                        {
                            result = new List<ISendMessage> { new TextMessage("https://line.me/S/sticker/14627819") };
                            result.Add(new ImageMessage("https://res.cloudinary.com/dtghn46kc/image/upload/v1676197737/monSticker_f6giu7.png", "https://res.cloudinary.com/dtghn46kc/image/upload/v1676197737/monSticker_f6giu7.png"));
                        }
                        else if (messageText == "我想買茗爺家傳肉乾")
                        {
                            result = new List<ISendMessage> { new TextMessage("Line官方帳號訂購 : https://liff.line.me/1645278921-kWRPP32q?accountId=rcf5071r&openerPlatform=native&openerKey=qrcode") };
                            result.Add(new ImageMessage("https://res.cloudinary.com/dtghn46kc/image/upload/v1676197812/MEAT_wru5fk.jpg", "https://res.cloudinary.com/dtghn46kc/image/upload/v1676197812/MEAT_wru5fk.jpg"));
                        }
                        else if (messageText == "我想買黑狗軒免運布朗尼")
                        {
                            result = new List<ISendMessage> { new TextMessage("TKS 提克斯國際 三倍濃心布朗尼，黑狗軒真心免運推薦 ! : https://shopee.tw/feddle") };
                            result.Add(new TextMessage("Line官方帳號訂購 : https://line.me/ti/p/6ykS4T68B0"));
                            result.Add(new ImageMessage("https://res.cloudinary.com/dtghn46kc/image/upload/v1676197812/BLACK_wh4b60.png", "https://res.cloudinary.com/dtghn46kc/image/upload/v1676197812/BLACK_wh4b60.png"));
                        }
                        else if (messageText == "集點")
                        {
                            result = new List<ISendMessage> { new TextMessage("集❕❕❕") };
                            result.Add(new TextMessage("https://liff.line.me/1654883656-XqwKRkd4?aid=760pvdld&utm_source=LINE&utm_medium=Owner&utm_campaign=Share"));
                        }
                        //圖文選單內容
                        else if (messageText == "想加入張育仁之家")
                        {
                            result = new List<ISendMessage> { (new TextMessage("自己去問系辦啦廢物")) };
                        }
                        else if (messageText == "正偉老師說")
                        {
                            result = new List<ISendMessage> { new VideoMessage("https://res.cloudinary.com/dtghn46kc/video/upload/v1675596962/%E6%AD%A3%E5%81%89%E5%A4%A7%E4%B8%80%E8%B3%87%E7%B5%90%E5%A5%BD%E9%87%8D%E8%A6%81_efvq0o.mp4", "https://upload.cc/i1/2023/02/05/edoGOz.jpg") };
                            result.Add(new TextMessage("( 👨🏻‍🦲大一資結好重要 )"));
                        }
                        else if (messageText == "班導的話")
                        {
                            result = new List<ISendMessage> { new TextMessage("\"'  '  '  '  '  '  '  '\n希望我們的同學畢業後，能夠發揮所長，齁\U0001f618\n\n發揮所長並不表示你只能做資管相關的工作，齁，發揮所長，代表的是全人的發揮，齁\U0001f44a\n\n比如說同學可能外燴經營很有興趣\U0001f373、有的同學想要當健身教練\U0001f4aa、或是買賣保險\U0001f4b9，這都是會很有成就的，齁，只要你發揮所長，齁!\n\n那我們在東海求學呢，學問其實只是一個基礎，就是你多讀一些書\U0001f4D6，對你將來會產生比較好的想法並進行規劃，齁。\n\n這是我身為你們導師所希望看到的齁，而不是說今天讀資管就一定要做資管相關工作，一定要做programmer\U0001f4BB，不是這樣的，齁\U0001f616\n\n我希望大家都能發揮所長，那我會非常的proud of everybody。那記得如果你發揮所長了，記得多參加同學會與昔日的好友們多多交流，或是回來學校看看老師，齁\U0001f64F\n\n不要覺得甚麼，唉呦我發揮所長後跟資管無關跟同學沒話題或是愧對老師，不要醬啦，齁\U0001f612\n\n其實只要你發揮所長，我們都是資管的學生，我們都會非常以你為榮，齁 ✌✌✌\n\n'  '  '  '  '  '  '  '\"") };
                        }
                        else if (messageText == "誰是昂哥")
                        {
                            result = new List<ISendMessage> { new TextMessage("\U0001F409東海龍王昂哥，是現任高雄苗栗屏東花蓮及新北三重蘆洲地方大老、企業公安管理顧問師、海軍陸戰隊培訓師、職業演說家、並經營一間AI砂石場，每年吸引超過20億的資金投資。\n\n☠真槍實彈實戰經驗23年，商業經營15年。從陌生派系開發一路經營到社會上流，專精於地方營運、公安設計、組織經營與管理。\n\n☢服務過的客戶包含伊拉克、美國蛙人部隊、俄羅斯北極熊生擒小隊、亞馬遜食人魚活烤三吃小隊、越南科摩多龍生擒小隊、中國新冠肺炎生擒小隊、…等百大特殊單位。\n\n\U0001F479昂哥公司協助各級組織創建永續發展的地方經營策略，致力於訓練、建立、指導高績效團隊。他以其專業領域「戰鬥哲學」為基礎，設計出獨特的指導手法，以最簡單直白的方式，協助各行各業中埋伏的內賊及地方樁腳大幅提升資產，廣受好評。") };
                            result.Add(new ImageMessage("https://res.cloudinary.com/dtghn46kc/image/upload/v1676197682/%E6%9D%B1%E6%B5%B7%E9%BE%8D%E7%8E%8B%E6%98%82%E5%93%A5_yg3xqv.jpg", "https://res.cloudinary.com/dtghn46kc/image/upload/v1676197682/%E6%9D%B1%E6%B5%B7%E9%BE%8D%E7%8E%8B%E6%98%82%E5%93%A5_yg3xqv.jpg"));
                        }
                        else if (messageText == "Hi" || messageText == "你好" || messageText == "hi" || messageText == "嗨")
                        {
                            result = new List<ISendMessage> { new TextMessage("你好啊") };
                        }
                        else if (messageText == "我們最愛的師長")
                        {                            
                            result2 = new ReplyMessageRequestDto<TemplateMessageModel<ImageCarouselTemplateDto>>
                            {
                                ReplyToken = ev.ReplyToken,
                                Messages = new List<TemplateMessageModel<ImageCarouselTemplateDto>>
                                {
                                    new TemplateMessageModel<ImageCarouselTemplateDto>
                                    {
                                        AltText = "我們最愛的師長",
                                        Template = new ImageCarouselTemplateDto
                                        {
                                            Columns = new List<ImageCarouselColumnObjectDto>
                                            {
                                                new ImageCarouselColumnObjectDto
                                                {
                                                    ImageUrl = "https://res.cloudinary.com/dtghn46kc/image/upload/v1676197539/%E6%AD%A3%E5%81%89%E8%80%81%E5%B8%AB_iy1sql.jpg",
                                                    Action = new ActionDto
                                                    {
                                                        Type = ActionTypeEnum.Message,
                                                        Label = "正偉老師說",
                                                        Text = "正偉老師說"
                                                    }
                                                },
                                                new ImageCarouselColumnObjectDto
                                                {
                                                    ImageUrl = "https://res.cloudinary.com/dtghn46kc/image/upload/v1676197538/%E7%9B%9B%E7%A8%8B%E8%80%81%E5%B8%AB_zvee9y.jpg",
                                                    Action = new ActionDto
                                                    {
                                                        Type = ActionTypeEnum.Message,
                                                        Label = "班導的話",
                                                        Text = "班導的話"
                                                    }
                                                },
                                                new ImageCarouselColumnObjectDto
                                                {
                                                    ImageUrl = "https://res.cloudinary.com/dtghn46kc/image/upload/v1676197539/%E8%87%AA%E5%BC%B7%E8%80%81%E5%B8%AB_ik2smo.jpg",
                                                    Action = new ActionDto
                                                    {
                                                        Type = ActionTypeEnum.Uri,
                                                        Label = "訂閱姜老師🄰🄸頻道",
                                                        Uri = "https://liff.line.me/1656959733-5gyYdjQx"
                                                    }
                                                },
                                                new ImageCarouselColumnObjectDto
                                                {
                                                    ImageUrl = "https://res.cloudinary.com/dtghn46kc/image/upload/v1676197539/%E5%BF%83%E6%B7%B3%E8%80%81%E5%B8%AB_smjphu.jpg",
                                                    Action = new ActionDto
                                                    {
                                                        Type = ActionTypeEnum.Uri,
                                                        Label = "看看心淳老師教學多棒♥",
                                                        Uri = "https://liff.line.me/1656959733-PWgpqDv3"
                                                    }
                                                },
                                                new ImageCarouselColumnObjectDto
                                                {
                                                    ImageUrl = "https://res.cloudinary.com/dtghn46kc/image/upload/v1676197539/%E8%82%B2%E4%BB%81%E8%80%81%E5%B8%AB_dfsbk4.jpg",
                                                    Action = new ActionDto
                                                    {
                                                        Type = ActionTypeEnum.Message,
                                                        Label = "想加入張育仁之家",
                                                        Text = "想加入張育仁之家"
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            };

                        }
                        else
                        {
                            result = new List<ISendMessage> { new TextMessage("我不懂你的訊息QQ 也許你是想了解~", quickReply) };
                        }
                    }
                    break;

                //貼圖訊息
                case Line.Messaging.Webhooks.StickerEventMessage stickerEventMessage:
                    {
                        result = new List<ISendMessage> { new TextMessage("最強的昂哥貼圖 💪💪") };
                        result.Add(new TextMessage("https://line.me/S/sticker/14627819"));
                    }
                    break;
            }

            if (result != null)
                await _messagingClient.ReplyMessageAsync(ev.ReplyToken, result);
            if (result2 != null)
                ReplyMessageHandler(result2);
        }
    }
}
