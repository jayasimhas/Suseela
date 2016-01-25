@echo off
setlocal enabledelayedexpansion

set websiteDirectory=%1
set includeDirectory=%websiteDirectory%\App_Config\Include
echo Enabling Solr in %websiteDirectory%

set filesToDisable[0]=Sitecore.ContentSearch.Lucene.Index.Analytics.config
set filesToDisable[1]=Sitecore.ContentSearch.Lucene.Index.Core.config
set filesToDisable[2]=Sitecore.ContentSearch.Lucene.Index.Master.config
set filesToDisable[3]=Sitecore.ContentSearch.Lucene.Index.Web.config
set filesToDisable[4]=ContentTesting\Sitecore.ContentTesting.Lucene.IndexConfiguration.config
set filesToDisable[5]=FXM\Sitecore.FXM.Lucene.DomainsSearch.DefaultIndexConfiguration.config
set filesToDisable[6]=ListManagement\Sitecore.ListManagement.Lucene.Index.List.config
set filesToDisable[7]=ListManagement\Sitecore.ListManagement.Lucene.IndexConfiguration.config
set filesToDisable[8]=Social\Sitecore.Social.Lucene.Index.Master.config
set filesToDisable[9]=Social\Sitecore.Social.Lucene.Index.Web.config
set filesToDisable[10]=Social\Sitecore.Social.Lucene.IndexConfiguration.config
set filesToDisable[11]=Sitecore.Marketing.Definitions.MarketingAssets.Repositories.Lucene.Index.Master.config
set filesToDisable[12]=Sitecore.Marketing.Definitions.MarketingAssets.Repositories.Lucene.Index.Web.config
set filesToDisable[13]=Sitecore.Marketing.Definitions.MarketingAssets.Repositories.Lucene.IndexConfiguration.config
set filesToDisable[14]=Sitecore.Marketing.Lucene.Index.Master.config
set filesToDisable[15]=Sitecore.Marketing.Lucene.Index.Web.config
set filesToDisable[16]=Sitecore.Marketing.Lucene.IndexConfiguration.config
set filesToDisable[17]=FXM\Sitecore.FXM.Lucene.DomainsSearch.Index.Master.config
set filesToDisable[18]=FXM\Sitecore.FXM.Lucene.DomainsSearch.Index.Web.config
set filesToDisable[19]=Sitecore.ContentSearch.Lucene.DefaultIndexConfiguration.config

echo Disabling Lucene configuration files

for /l %%n in (0,1,19) do (
echo Disabling file: %includeDirectory%\!filesToDisable[%%n]!
move "%includeDirectory%\!filesToDisable[%%n]!" "%includeDirectory%\!filesToDisable[%%n]!.disabled"
)

set fileToEnable[0]=Sitecore.ContentSearch.Solr.DefaultIndexConfiguration.config
set fileToEnable[1]=Sitecore.ContentSearch.Solr.Index.Analytics.config
set fileToEnable[2]=Sitecore.ContentSearch.Solr.Index.Core.config
set fileToEnable[3]=Sitecore.ContentSearch.Solr.Index.Master.config
set fileToEnable[4]=Sitecore.ContentSearch.Solr.Index.Web.config
set fileToEnable[5]=ContentTesting\Sitecore.ContentTesting.Solr.IndexConfiguration.config
set fileToEnable[6]=FXM\Sitecore.FXM.Solr.DomainsSearch.DefaultIndexConfiguration.config
set fileToEnable[7]=ListManagement\Sitecore.ListManagement.Solr.Index.List.config
set fileToEnable[8]=ListManagement\Sitecore.ListManagement.Solr.IndexConfiguration.config
set fileToEnable[9]=Social\Sitecore.Social.Solr.Index.Master.config
set fileToEnable[10]=Social\Sitecore.Social.Solr.Index.Web.config
set fileToEnable[11]=Social\Sitecore.Social.Solr.IndexConfiguration.config
set fileToEnable[12]=Sitecore.Marketing.Definitions.MarketingAssets.Repositories.Solr.Index.Master.config
set fileToEnable[13]=Sitecore.Marketing.Definitions.MarketingAssets.Repositories.Solr.Index.Web.config
set fileToEnable[14]=Sitecore.Marketing.Definitions.MarketingAssets.Repositories.Solr.IndexConfiguration.config
set fileToEnable[15]=Sitecore.Marketing.Solr.Index.Master.config
set fileToEnable[16]=Sitecore.Marketing.Solr.Index.Web.config
set fileToEnable[17]=Sitecore.Marketing.Solr.IndexConfiguration.config
set fileToEnable[18]=FXM\Sitecore.FXM.Solr.DomainsSearch.Index.Master.config
set fileToEnable[19]=FXM\Sitecore.FXM.Solr.DomainsSearch.Index.Web.config

echo Enabling Solr configuration files

for /l %%n in (0,1,19) do (
echo Disabling file: %includeDirectory%\!fileToEnable[%%n]!
move "%includeDirectory%\!fileToEnable[%%n]!.disabled" "%includeDirectory%\!fileToEnable[%%n]!"
move "%includeDirectory%\!fileToEnable[%%n]!.example" "%includeDirectory%\!fileToEnable[%%n]!"
)

echo Solr installed