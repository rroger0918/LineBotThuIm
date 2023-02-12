using Line.Messaging;
using System;
using System.Collections.Generic;

namespace LineBotThuIm
{
    public class TemplateMessageModel<T> : BaseMessageModel
    {
        public TemplateMessageModel()
        {
            Type = MessageTypeEnum.Template;
        }

        public string AltText { get; set; }
        public T Template { get; set; }
    }

    public class ButtonsTemplateDto
    {
        public string Type { get; set; } = TemplateTypeEnum.Buttons;
        public string Text { get; set; }
        public List<ActionDto>? Actions { get; set; }

        public string? ThumbnailImageUrl { get; set; }
        public string? ImageAspectRatio { get; set; }
        public string? ImageSize { get; set; }
        public string? ImageBackgroundColor { get; set; }
        public string? Title { get; set; }
        public string? DefaultAction { get; set; }
    }

    public class ConfirmTemplateDto
    {
        public string Type { get; set; } = TemplateTypeEnum.Confirm;
        public string Text { get; set; }
        public List<ActionDto>? Actions { get; set; }
    }

    public class CarouselTemplateDto
    {
        public string Type { get; set; } = TemplateTypeEnum.Carousel;
        public List<CarouselColumnObjectDto> Columns { get; set; }

        public string ImageAspectRatio { get; set; }
        public string ImageSize { get; set; }
    }

    public class CarouselColumnObjectDto
    {
        public string Text { get; set; }
        public List<ActionDto> Actions { get; set; }

        public string? ThumbnailImageUrl { get; set; }
        public string? ImageBackgroundColor { get; set; }
        public string? Title { get; set; }
        public ActionDto? DefaultAction { get; set; }
    }

    //圖片輪播模板
    public class ImageCarouselTemplateDto
    {
        public string Type { get; set; } = TemplateTypeEnum.ImageCarousel;
        public List<ImageCarouselColumnObjectDto> Columns { get; set; }
    }

    public class ImageCarouselColumnObjectDto
    {
        public string ImageUrl { get; set; }
        public ActionDto Action { get; set; }

    }

    public static class TemplateTypeEnum
    {
        public const string Buttons = "buttons";
        public const string Confirm = "confirm";
        public const string Carousel = "carousel";
        public const string ImageCarousel = "image_carousel";
    }

    public static class MessageTypeEnum
    {
        public const string Text = "text";
        public const string Sticker = "sticker";
        public const string Image = "image";
        public const string Video = "video";
        public const string Audio = "audio";
        public const string Location = "location";
        public const string Imagemap = "imagemap";
        public const string Template = "template";
        public const string Flex = "flex";
    }

    public class ActionDto
    {
        public string Type { get; set; }
        public string? Label { get; set; }

        //Postback action.
        public string? Data { get; set; }
        public string? DisplayText { get; set; }
        public string? InputOption { get; set; }
        public string? FillInText { get; set; }

        //Message action.
        public string Text { get; set; }

        //Uri action.
        public string Uri { get; set; }
    }

    public class ReplyMessageRequestDto<T>
    {
        public string ReplyToken { get; set; }
        public List<T> Messages { get; set; }
    }

    public static class ActionTypeEnum
    {
        public const string Postback = "postback";
        public const string Message = "message";
        public const string Uri = "uri";
        public const string DatetimePicker = "datetimepicker";
        public const string Camera = "camera";
        public const string CameraRoll = "cameraRoll";
        public const string Location = "location";
        public const string RichMenuSwitch = "richmenuswitch";
    }
}
