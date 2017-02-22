using Sitecore;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Web;
using Sitecore.Web.UI.Pages;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Web.UI.HtmlControls;
using HtmlAgilityPack;
using System.Xml.Linq;

namespace Informa.Models.Customization
{
    public class InsertIframe : DialogForm
    {
        //fields
        protected Memo memIframeDesktop;
        protected Memo memIframeMobile;
        protected Edit memIframeHeader;
        protected Edit memIframeTitle;
        protected Edit memIframeCaption;
        protected Edit memIframeSource;

        // Properties
        protected string Mode
        {
            get
            {
                //string str = StringUtil.GetString(base.ServerProperties["Mode"]);
                //if (!string.IsNullOrEmpty(str))
                //{
                //    return str;
                //}
                return "shell";
            }
            set
            {
                Assert.ArgumentNotNull(value, "value");
                //base.ServerProperties["Mode"] = value;
            }
        }

        //setup page
        protected override void OnLoad(EventArgs e)
        {           
            Assert.ArgumentNotNull(e, "e");
            base.OnLoad(e);
            if (!Context.ClientPage.IsEvent)
            {
                //this.Mode = WebUtil.GetQueryString("mo");
                string text = WebUtil.GetQueryString("selectedText");               
            }
        }

        /// <summary>
        /// When pressed Insert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void OnOK(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");


            string desktopIframeCode = memIframeDesktop.Value;
            string mobileIframeCode = memIframeMobile.Value;
            if (!string.IsNullOrEmpty(desktopIframeCode))
            {
                var newNode = HtmlNode.CreateNode(desktopIframeCode);
                if (IsSecure(newNode))
                {
                    string html = BuildHtml().ToString();
                    //encode it and send it back to the rich text editor
                    if (this.Mode == "webedit")
                    {
                        SheerResponse.SetDialogValue(StringUtil.EscapeJavascriptString(html));
                        base.OnOK(sender, args);
                    }
                    else
                    {
                        SheerResponse.Eval("scClose(" + StringUtil.EscapeJavascriptString(html) + ")");
                    }
                }
                else
                    SheerResponse.Alert("Multimedia content is either missing, invalid or not secure", new string[0]);

            }
            else
            {
                SheerResponse.Alert("Multimedia content is either missing, invalid or not secure", new string[0]);
            }

        }

        /// <summary>
        /// Checks if a secured iframe code is entered
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool IsSecure(HtmlNode node)
        {
            string source = node.GetAttributeValue("src", "");
            if (!string.IsNullOrEmpty(source) && source.Contains("https"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// When pressed cancelled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void OnCancel(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");
            if (this.Mode == "webedit")
            {
                base.OnCancel(sender, args);
            }
            else
            {
                SheerResponse.Eval("scCancel()");
            }
        }

        /// <summary>
        /// On click of insert, paste this html to the RTE editor
        /// </summary>
        /// <returns></returns>
        private XElement BuildHtml()
        {
            string iframeGroupId = String.Empty;
            string cssStyle = string.Empty;
            var xData = new XElement("div");
            xData.SetAttributeValue("class", "root");
            iframeGroupId = Guid.NewGuid().ToString("N");

            if (!string.IsNullOrEmpty(memIframeHeader.Value))
            {
                var paraHeaderElement = ParagraphStyles("header", iframeGroupId, memIframeHeader.Value);
                xData.Add(paraHeaderElement);
            }

            if (!string.IsNullOrEmpty(memIframeTitle.Value))
            {
                var paraTitleElement = ParagraphStyles("Title", iframeGroupId, memIframeTitle.Value);
                xData.Add(paraTitleElement);
            }


            XElement iframeGroupElement = new XElement("div");
            iframeGroupElement.SetAttributeValue("class", "iframe-component");


            if (!string.IsNullOrEmpty(memIframeDesktop.Value))
            {
                var iframeDesktopElement = MobileOrDesktopEmbed(iframeGroupId);
                iframeDesktopElement.SetAttributeValue("data-embed-link", "enabled");
                iframeGroupElement.Add(iframeDesktopElement);
            }

            if (!string.IsNullOrEmpty(memIframeMobile.Value))
            {
                var iframeMobileElement = MobileOrDesktopEmbed(iframeGroupId, true);               
                iframeMobileElement.SetAttributeValue("data-embed-link", "enabled");
                iframeGroupElement.Add(iframeMobileElement);                
            }
            else
            {
                var openMediaLink = OpenMediaLink(iframeGroupId);
                openMediaLink.SetAttributeValue("data-embed-link", "enabled");
                iframeGroupElement.Add(openMediaLink);
            }

            xData.Add(iframeGroupElement);
            if (!string.IsNullOrEmpty(memIframeCaption.Value))
            {
                var paraCaptionElement = ParagraphStyles("Caption", iframeGroupId, memIframeCaption.Value);
                xData.Add(paraCaptionElement);
            }

            if (!string.IsNullOrEmpty(memIframeSource.Value))
            {
                var paraSourceElement = ParagraphStyles("Source", iframeGroupId, memIframeSource.Value);
                xData.Add(paraSourceElement);
            }

            return xData;
        }


        /// <summary>
        /// all <para> </para> tag styles
        /// </summary>
        /// <param name="stylesection"></param>
        /// <param name="iframeGroupId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private XElement ParagraphStyles(string stylesection, string iframeGroupId, string value)
        {
            XElement curElement = new XElement("p");
            string iframeIdClass = String.Format("iframe-{0}_{1}", stylesection,
                       iframeGroupId);
            curElement.SetAttributeValue("class",
                       String.Format("article-exhibit__{0} {1}",
                           stylesection,
                           iframeIdClass));
            curElement.Value = value;

            return curElement;
        }

        /// <summary>
        /// When mobile embed is empty
        /// </summary>
        /// <param name="iframeGroupId"></param>
        /// <returns></returns>
        private XElement OpenMediaLink(string iframeGroupId)
        {

            var iframeElement = new XElement("div");
            string cssStyle = String.Format("ewf-mobile-iframe_{0}", iframeGroupId);
            iframeElement.SetAttributeValue("class", $"iframe-component__mobile {cssStyle}");
           
            var link = new XElement("a");
            link.SetAttributeValue("href", "");
            link.SetAttributeValue("class", "iframe-component__desktop-showcase-link");
            link.SetAttributeValue("onclick", "window.open($(this).data('mediaid')); return false;");
            link.Value = "Open Media";

            iframeElement.Add(link);
            return iframeElement;
        }

        /// <summary>
        /// Checks if mobile or destop embed and generates the html based on condition
        /// </summary>
        /// <param name="iframeGroupId"></param>
        /// <param name="isMobile"></param>
        /// <returns></returns>

        private XElement MobileOrDesktopEmbed(string iframeGroupId, bool isMobile = false)
        {

            var iframeElement = new XElement("div");
            //var iframeDesktop = new XElement("iframe");
            var desktopNode = HtmlNode.CreateNode(memIframeDesktop.Value);
            var mobileNode = HtmlNode.CreateNode(memIframeMobile.Value);

            if (isMobile)
            {
                var iframeMobile = XElement.Parse(memIframeMobile.Value);
                string cssStyle = String.Format("ewf-mobile-iframe_{0}", iframeGroupId);
                iframeMobile.SetAttributeValue("class", cssStyle);
                iframeElement.SetAttributeValue("class", $"iframe-component__mobile {cssStyle}");
                iframeElement.Add(iframeMobile);
                return iframeElement;
            }
            else
            {
                var iframeDesktop = XElement.Parse(memIframeDesktop.Value);
                string cssStyle = String.Format("ewf-desktop-iframe_{0}", iframeGroupId);
                iframeDesktop.SetAttributeValue("class", cssStyle);
                iframeElement.SetAttributeValue("class", $"iframe-component__desktop {cssStyle}");
                iframeElement.SetAttributeValue("data-mediaid", iframeGroupId);
                iframeElement.Add(iframeDesktop);
                return iframeElement;
            }
        }

    }
}