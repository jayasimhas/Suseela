﻿using System;
using System.Windows.Forms;
using HtmlAgilityPack;
using Microsoft.Office.Interop.Word;
using SitecoreTreeWalker.Properties;
using SitecoreTreeWalker.Sitecore;
using SitecoreTreeWalker.Util;
using SitecoreTreeWalker.Util.Document;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using UserControl = System.Windows.Forms.UserControl;


namespace SitecoreTreeWalker.UI.TreeBrowser.TreeBrowserControls
{
    public partial class IFrameControl : UserControl
	{
		protected SitecoreItemGetter SiteCoreItemGetter;
		protected static IFrameControl IframeForm;
		protected static HtmlDocument MobileIFrame;
		protected static HtmlDocument DesktopIFrame;
		protected string InsecureDesktopURL;
		protected string InsecureMobileURL;
		private string _desktopPHText = "Copy and paste the code for your IFrame on desktop here, (required)";
		private string _mobilePHText = "Copy and paste the code for your IFrame on mobile here, (optional)";
		public IFrameControl()
		{
			InitializeComponent();
		}

		public void SetSitecoreItemGetter(SitecoreItemGetter siteCoreItemGetter)
		{
			SiteCoreItemGetter = siteCoreItemGetter;
		}
		
		/// <summary>
		/// Removes the current items in the TreeBrowser and re-initializes them.
		/// </summary>
		private void IFrameControl_Load(object sender, System.EventArgs e)
		{
			mobileEmbed.Text = _mobilePHText;
			desktopEmbed.Text = _desktopPHText;
			
			if (!DesignMode)
			{
				SetSitecoreItemGetter(new SitecoreItemGetter());
				
			}
		}

		private void uxInsertIFrame_Click(object sender, System.EventArgs e)
		{
			HideErrors();
			bool mobileErrors = false;
			bool desktopErrors = false;
			if (string.IsNullOrEmpty(desktopEmbed.Text) || desktopEmbed.Text.Trim().Equals(_desktopPHText))
			{
				uxDesktopError.Text = "Desktop code is required.";
				uxDesktopError.Show();
				desktopErrors = true;
			}

			if (!desktopErrors && !ValidDesktopInput(desktopEmbed.Text) )
			{
				uxDesktopError.Text = Resources.IFrameControl_uxInsertIFrame_Click_DesktopEmbedd;
				uxDesktopError.Show();	
				desktopErrors = true;
			}

			
			if (DesktopIFrame != null && !desktopErrors && !IFramesIsSecure(DesktopIFrame.DocumentNode.SelectNodes("//iframe"),out InsecureDesktopURL))
			{
				uxDesktopError.Text = Resources.IFrameControl_uxInsertIFrame_Click_Insecure_Multimedia;
				uxDesktopError.Show();
				uxDesktophttpsPreview.Visible = true;
				desktopErrors = true;
			}

			
			if (!ValidMobileInput(mobileEmbed.Text))
			{
				uxMobileError.Text = Resources.IFrameControl_uxInsertIFrame_Click_DesktopEmbedd;
				uxMobileError.Show();
				mobileErrors = true;
			}

			if (MobileIFrame != null && !mobileErrors && !IFramesIsSecure(MobileIFrame.DocumentNode.SelectNodes("//iframe"), out InsecureMobileURL))
			{
				uxMobileError.Text =
					Resources.IFrameControl_uxInsertIFrame_Click_Insecure_Multimedia;
				uxMobileError.Show();
				uxMobilehttpsPreview.Visible = true;
				mobileErrors = true;
			}

			if (!mobileErrors && !desktopErrors)
			{
				mobileEmbed.Text = mobileEmbed.Text.Trim().Equals(_mobilePHText) ? String.Empty : mobileEmbed.Text;//set mobile text to empty if it is the placeholder text
				InsertIFrame(uxIFrameHeader.Text, uxIFrameTitle.Text, uxIFrameCaption.Text, uxIFrameSource.Text, desktopEmbed.Text,
				             mobileEmbed.Text);				
			}
			else
			{
				MessageBox.Show(Resources.IFrameControl_uxInsertIFrame_Click_Multimedia_Error);
			}
		}

		private void HideErrors()
		{
			uxMobileError.Hide();
			uxMobilehttpsPreview.Hide();
			uxDesktopError.Hide();
			uxDesktophttpsPreview.Hide();
		}
		#region validation
		private bool ValidMobileInput(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return true;
			}

			return ValidInput(input, out MobileIFrame);
		}

		private bool ValidDesktopInput(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return false;
			}

			return ValidInput(input, out DesktopIFrame);
		}

		private bool ValidInput(string input, out HtmlDocument document)
		{
			if (string.IsNullOrEmpty(input))
			{
				document = null;
				return true;
			}
			
			HtmlNode.ElementsFlags.Remove("form"); //forces froms to have opening and closing tag
			string html = string.Format("<html><head></head><body>{0}</body></html>", input);
			document = new  HtmlDocument();
			document.LoadHtml(html);
			var scriptNodes = document.DocumentNode.SelectNodes("//script");
			var linkNodes = document.DocumentNode.SelectNodes("//link");
			var formNodes = document.DocumentNode.SelectNodes("//form");
			var styleNodes = document.DocumentNode.SelectNodes("//style");

			return (formNodes == null || formNodes.Count == 0)
			       && (scriptNodes == null || scriptNodes.Count == 0)
			       && (linkNodes == null || linkNodes.Count == 0)
			       && (styleNodes == null || styleNodes.Count == 0);
		}


		private bool IFramesIsSecure(HtmlNodeCollection htmlNodeCollection, out string inscureUrl)
		{
			HtmlNode.ElementsFlags.Remove("form"); //forces froms to have opening and closing tag
			inscureUrl = "";
			if (htmlNodeCollection == null)
			{
				return true;
			}

			foreach (HtmlNode node in htmlNodeCollection)
			{
				if (node.Attributes["src"] !=null && !string.IsNullOrEmpty(node.Attributes["src"].Value))
				{
					string source = node.Attributes["src"].Value;
					if (!source.StartsWith("https") && !source.StartsWith("//"))
					{
						inscureUrl = source;
						return false;
					}
				}
			}
			return true;
		}

		#endregion  

		private static void InsertIFrame(string header, string title, string caption, string source, string desktopEmbed,
		                                 string mobileEmbed)
		{
			var app = Globals.SitecoreAddin.Application;
			int numParagraph = 2;
			if (!string.IsNullOrEmpty(header)) numParagraph = numParagraph + 2;
			if (!string.IsNullOrEmpty(desktopEmbed)) numParagraph = numParagraph + 3;
			numParagraph = numParagraph + 2; //add from mobile code there will always be a message
			if (!string.IsNullOrEmpty(title)) numParagraph = numParagraph + 2;
			if (!string.IsNullOrEmpty(caption)) numParagraph = numParagraph + 2;
			if (!string.IsNullOrEmpty(source)) numParagraph = numParagraph + 2;
			for (int i = 0; i < numParagraph + 1; i++)
			{

				app.Selection.TypeParagraph();
			}

			app.Options.AutoFormatReplaceQuotes = false;
			app.Options.AutoFormatAsYouTypeReplaceQuotes = false;
			Range selection = app.Selection.Previous(WdUnits.wdParagraph, numParagraph);

			
			if (!string.IsNullOrWhiteSpace(header))
			{
				selection.Text = header;
				CreateCustomStyleFromBase(app, DocumentAndParagraphStyles.ExhibitNumberStyle,DocumentAndParagraphStyles.IFrameHeaderStyle);
				selection.set_Style(DocumentAndParagraphStyles.IFrameHeaderStyle);
				selection = selection.Next(WdUnits.wdParagraph);
			}

			if (!string.IsNullOrEmpty(title))
			{
				selection.Text = title;

				CreateCustomStyleFromBase(app, DocumentAndParagraphStyles.ExhibitTitleStyle, DocumentAndParagraphStyles.IFrameTitleStyle);
				selection.set_Style(DocumentAndParagraphStyles.IFrameTitleStyle);
				selection = selection.Next(WdUnits.wdParagraph);
			}

			CreateIFrameCodeStyle(app);


			if (!string.IsNullOrEmpty(desktopEmbed))
			{
				var desktopBodyNode= DesktopIFrame.DocumentNode.SelectSingleNode("//body");

				selection.Text = desktopBodyNode.InnerHtml;// desktopEmbed;
				selection.set_Style(DocumentAndParagraphStyles.IFrameCodeStyle);
				
				
				selection = selection.Next(WdUnits.wdParagraph);
			}


			if (!string.IsNullOrEmpty(mobileEmbed))
			{
				var mobileBodyNode = MobileIFrame.DocumentNode.SelectSingleNode("//body");
				selection.Text = mobileBodyNode.InnerHtml; //mobileEmbed;
			}
			else
			{
				selection.Text = IFrameEmbedBuilder.MobileMessage;
			}
			
			selection.set_Style(DocumentAndParagraphStyles.IFrameMobileCodeStyle);
			selection = selection.Next(WdUnits.wdParagraph);


			if (!string.IsNullOrEmpty(caption))
			{
				selection.Text = caption;
				CreateCustomStyleFromBase(app, DocumentAndParagraphStyles.ExhibitCaptionStyle52, DocumentAndParagraphStyles.IFrameCaptionStyle);
				selection.set_Style(DocumentAndParagraphStyles.IFrameCaptionStyle);
				
				selection = selection.Next(WdUnits.wdParagraph);
			}

			if (!string.IsNullOrWhiteSpace(source))
			{
				selection.Text = source;
				
				CreateCustomStyleFromBase(app, DocumentAndParagraphStyles.SourceStyle, DocumentAndParagraphStyles.IFrameSourceStyle);
				selection.set_Style(DocumentAndParagraphStyles.IFrameSourceStyle);
				selection.Next(WdUnits.wdParagraph).Select();
			}
		}


		/*
			 * TODO : There is no way to check if a style exists so we have to try to create it and 
			 then ignore the "Style alredy exists" error with the empty catch 
			 my apologies
			 * */
		private static void CreateCustomStyleFromBase(Microsoft.Office.Interop.Word.Application app, string baseStyle,
		                                               string newStyleName)
		{
			using (new AlertDisabler(app))
			{
				try
				{
					var style = app.ActiveDocument.Styles.Add(newStyleName);
					style.set_BaseStyle(baseStyle);
					
				}
				catch
				{
				}
			}
			
		}
		private static void CreateIFrameCodeStyle(Microsoft.Office.Interop.Word.Application app )
		{

			using (new AlertDisabler(app))
			{

				try
				{
					var style = app.ActiveDocument.Styles.Add(DocumentAndParagraphStyles.IFrameCodeStyle);
					style.set_BaseStyle(DocumentAndParagraphStyles.ExhibitNumberStyle);
					style.set_NextParagraphStyle(DocumentAndParagraphStyles.StoryText22);
					style.Font.Size = (float) 10.5;
					style.Font.Name = "Consolas";
				}
				catch
				{
				}

				try
				{
					var style = app.ActiveDocument.Styles.Add(DocumentAndParagraphStyles.IFrameMobileCodeStyle);

					style.set_BaseStyle(DocumentAndParagraphStyles.ExhibitNumberStyle);
					style.set_NextParagraphStyle(DocumentAndParagraphStyles.StoryText22);
					style.Font.Size = (float) 10.5;
					style.Font.Name = "Consolas";

				}
				catch
				{
				}
			}
		}




		internal void uxMobileEmbed_LostFocus(object sender, EventArgs e)
		{
			if (mobileEmbed.Text.Trim().Equals(String.Empty))
			{
				mobileEmbed.Text = _mobilePHText;
			}
		}

		internal void uxDesktopEmbed_LostFocus(object sender, EventArgs e)
		{
			if (desktopEmbed.Text.Trim().Equals(String.Empty))
			{
				desktopEmbed.Text = _desktopPHText;
			}
		}

		internal void uxMobileEmbed_Focus(object sender, EventArgs e)
		{

			if (mobileEmbed.Text.Trim().Equals(_mobilePHText))
			{
				mobileEmbed.Text = String.Empty;
			}
		}
		internal void uxDesktopEmbed_Focus(object sender, EventArgs e)
		{
			
			if (desktopEmbed.Text.Trim().Equals(_desktopPHText))
			{
				desktopEmbed.Text = String.Empty;
			}
		}

		private void uxMobilehttpsPreview_Click(object sender, EventArgs e)
		{
			SuggestedURL.Open(InsecureMobileURL, this.InsertMobileUrl);
		}

		private void uxDesktophttpsPreview_Click(object sender, EventArgs e)
		{
			SuggestedURL.Open(InsecureDesktopURL,this.InsertDesktopUrl);
		}

		internal void InsertMobileUrl(string newUrl)
		{
			if (!String.IsNullOrEmpty(newUrl))
			{
				HtmlNode newIframeContent = ReplaceIFrameUrl(MobileIFrame, newUrl);
				if (newIframeContent != null)
				{
					mobileEmbed.Text = newIframeContent.OuterHtml;
					uxMobileError.Hide();
					uxMobilehttpsPreview.Hide();
				}
			}
		}

		internal void InsertDesktopUrl(string newUrl)
		{
			if (!String.IsNullOrEmpty(newUrl))
			{
				HtmlNode newIframeContent = ReplaceIFrameUrl(DesktopIFrame, newUrl);
				if (newIframeContent != null)
				{
					desktopEmbed.Text = newIframeContent.OuterHtml;
					uxDesktopError.Hide();
					uxDesktophttpsPreview.Hide();
				}
			}
		}

		private HtmlNode ReplaceIFrameUrl(HtmlDocument iframe, string newUrl)
		{
			var htmlNodeCollection = iframe.DocumentNode.SelectNodes("//iframe");
			foreach (HtmlNode node in htmlNodeCollection)
			{
				if (node.Attributes["src"] != null && !string.IsNullOrEmpty(node.Attributes["src"].Value))
				{
					node.Attributes["src"].Value = newUrl;
					return node;
				} 
			}
			return null;
		}

		private void mobileEmbed_TextChanged(object sender, EventArgs e)
		{

		}

        private void header_TextChanged(object sender, EventArgs e)
        {
            headerLabel.Text = this.uxIFrameHeader.Text;            
        }

        private void title_TextChanged(object sender, EventArgs e)
        {
            titleLabel.Text = this.uxIFrameTitle.Text;
        }

        private void caption_TextChanged(object sender, EventArgs e)
        {
            captionLabel.Text = this.uxIFrameCaption.Text;
        }

        private void source_TextChanged(object sender, EventArgs e)
        {
            sourceLabel.Text = this.uxIFrameSource.Text;
        }
	}
}

