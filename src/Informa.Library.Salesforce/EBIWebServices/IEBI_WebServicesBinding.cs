﻿namespace Informa.Library.Salesforce.EBIWebServices
{
	using System;
	using System.Web.Services;
	using System.Diagnostics;
	using System.Web.Services.Protocols;
	using System.Xml.Serialization;
	using System.ComponentModel;


	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
	[System.Web.Services.WebServiceBindingAttribute(Name = "EBI_WebServicesBinding",
		Namespace = "http://soap.sforce.com/schemas/class/EBI_WebServices")]
	public interface IEBI_WebServicesBinding
	{
		SessionHeader SessionHeaderValue { get; set; }

		CallOptions CallOptionsValue { get; set; }

		DebuggingHeader DebuggingHeaderValue { get; set; }

		AllowFieldTruncationHeader AllowFieldTruncationHeaderValue { get; set; }

		DebuggingInfo DebuggingInfoValue { get; set; }

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_CreateAccountResponse createAccount(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] EBI_Name name,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] EBI_CompanyJob CompanyJob);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_CartResponse createCart([System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] EBI_Cart newCart);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_CartItemResponse createCartItems(
			[System.Xml.Serialization.XmlElementAttribute("newCartItems", IsNullable = true)] EBI_CartItem[] newCartItems);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_WebServiceResponse createOrder(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] EBI_Order newOrder);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_CreateProfileResponse createProfile(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] EBI_CreateProfileRequest createProfileRequest);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_WebServiceResponse createSavedDocument(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] EBI_SavedDocument savedDocument,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_WebServiceResponse createSavedSearchItem(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string searchString,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string name,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] System.Nullable<bool> IsReceivingEmailAlert);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_WebServiceResponse deleteSavedDocument(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string documentID);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_WebServiceResponse deleteSavedSearch(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string name);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_WebServiceResponse isCouponCodeValid(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string couponCode);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_LoginResponse login([System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName,
								[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string password);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_QueryAccountByIPAddressResponse queryAccountByIPAddress(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string ipAddress);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_QueryAccountByMasterIdResponse queryAccountByMasterId(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string masterId,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string masterPassword);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_QueryAccountBySubscriberIdResponse queryAccountBySubscriberId(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string subscriberId);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_QueryAccountByUsernameResponse queryAccountByUsername(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_QueryAccountByVerificationCodeResponse queryAccountByVerificationCode(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string verificationCode);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_QueryAllActiveIPRangesResponse queryAllActiveIPRanges();

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_QueryCartResponse queryCart([System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_QueryCartResponse queryCartById([System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string cartId);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_QueryEmailNewsletterOptInsResponse queryEmailNewsletterOptins(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_QueryEntitlementsResponse queryEntitlements(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string ipAddress);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_QueryFreeELetterOptInsResponse queryFreeELetterOptins(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_QueryInformationAndOfferOptInsResponse queryInformationAndOfferOptins(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_QueryOptInsResponse queryOptIns([System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_QueryProfileContactInformationResponse queryProfileContactInformation(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_QuerySavedDocumentResponse querySavedDocuments(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_QuerySavedSearchItemsResponse querySavedSearchItems(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_QueryEntitlementsResponse querySiteEntitlementsIP(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string ipAddress);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_QuerySubscriptionsAndPurchasesResponse querySubscriptionsAndPurchases(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_WebServiceResponse transferCart([System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string cartId,
											[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_WebServiceResponse updateBillingAddressAndPhone(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] EBI_Address billingAddress,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] EBI_PhoneFax billingPhoneFax);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_WebServiceResponse updateBillingName(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] EBI_Name billingName);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_CartResponse updateCart([System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] EBI_Cart newCart);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_CartItemResponse updateCartItems(
			[System.Xml.Serialization.XmlElementAttribute("newCartItems", IsNullable = true)] EBI_CartItem[] newCartItems);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_WebServiceResponse updateCompanyJob(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] EBI_CompanyJob companyJob);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_WebServiceResponse updateDoNotSendInformationAndOffers(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] System.Nullable<bool> doNotSendInformationAndOffers);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_WebServiceResponse updateEmailNewsletterOptIns(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName,
			[System.Xml.Serialization.XmlElementAttribute("emailNewsletterOptins", IsNullable = true)] EBI_EmailNewsLetterOptin[]
				emailNewsletterOptins);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_WebServiceResponse updateFreeELetterOptins(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName,
			[System.Xml.Serialization.XmlElementAttribute("freeELetterOptins", IsNullable = true)] string[] freeELetterOptins);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_WebServiceResponse updateInformationAndOfferOptins(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName,
			[System.Xml.Serialization.XmlElementAttribute("InformationAndOfferOptins", IsNullable = true)] string[]
				InformationAndOfferOptins);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_WebServiceResponse updatePassword(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string currentPassword,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] System.Nullable<bool> isTempPassword,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string newPassword);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_WebServiceResponse updatePhoneFax(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] EBI_PhoneFax phone);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_WebServiceResponse updateProfile(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] EBI_UpdateProfileRequest profileRequest);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_WebServiceResponse updateSavedSearchItem(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string searchString,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string Name,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] System.Nullable<bool> IsReceivingEmailAlert);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_WebServiceResponse updateShippingAddressAndPhone(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] EBI_Address shippingAddress,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] EBI_PhoneFax shippingPhoneFax);

		/// <remarks/>
		[System.Web.Services.Protocols.SoapHeaderAttribute("CallOptionsValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("SessionHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("AllowFieldTruncationHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingHeaderValue")]
		[System.Web.Services.Protocols.SoapHeaderAttribute("DebuggingInfoValue",
			Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
		[System.Web.Services.WebMethodAttribute()]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
			RequestNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			ResponseNamespace = "http://soap.sforce.com/schemas/class/EBI_WebServices",
			Use = System.Web.Services.Description.SoapBindingUse.Literal,
			ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("result", IsNullable = true)]
		EBI_WebServiceResponse updateShippingName(
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string userName,
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] EBI_Name ShippingName);
	}
}
