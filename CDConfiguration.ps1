$scriptDir = Split-Path (Resolve-Path $myInvocation.MyCommand.Path)

function Get-FilesToDisableOnCDServer()
{
    $files = @("\App_Config\Include\001.Sitecore.Speak.Important.config",
"\App_Config\Include\Sitecore.Analytics.Automation.TimeoutProcessing.config",
"\App_Config\Include\Sitecore.Analytics.Oracle.config.disabled",
"\App_Config\Include\Sitecore.Analytics.Processing.Aggregation.config",
"\App_Config\Include\Sitecore.Analytics.Processing.Aggregation.Services.config",
"\App_Config\Include\Sitecore.Analytics.Processing.config",
"\App_Config\Include\Sitecore.Analytics.Processing.Services.config",
"\App_Config\Include\Sitecore.Analytics.Reporting.config",
"\App_Config\Include\Sitecore.ExperienceEditor.Speak.Requests.config",
"\App_Config\Include\Sitecore.ExperienceExplorer.Speak.Requests.config",
"\App_Config\Include\Sitecore.Marketing.Client.config",
"\App_Config\Include\Sitecore.Marketing.Definitions.MarketingAssets.Repositories.Lucene.Index.Master.config",
"\App_Config\Include\Sitecore.MarketingProcessingRole.config.disabled",
"\App_Config\Include\Sitecore.MarketingReportingRole.config.disabled",
"\App_Config\Include\Sitecore.PathAnalyzer.Client.config",
"\App_Config\Include\Sitecore.PathAnalyzer.config",
"\App_Config\Include\Sitecore.PathAnalyzer.Processing.config",
"\App_Config\Include\Sitecore.PathAnalyzer.RemoteClient.config.disabled",
"\App_Config\Include\Sitecore.PathAnalyzer.Services.config",
"\App_Config\Include\Sitecore.PathAnalyzer.Services.RemoteServer.config.disabled",
"\App_Config\Include\Sitecore.PathAnalyzer.StorageProviders.config",
"\App_Config\Include\Sitecore.Processing.config",
"\App_Config\Include\Sitecore.Publishing.EventProvider.Async.config.disabled",
"\App_Config\Include\Sitecore.Publishing.Parallel.config.disabled",
"\App_Config\Include\Sitecore.Shell.MarketingAutomation.config",
"\App_Config\Include\Sitecore.Speak.AntiCsrf.SheerUI.config",
"\App_Config\Include\Sitecore.Speak.Applications.config",
"\App_Config\Include\Sitecore.Speak.Components.config",
"\App_Config\Include\Sitecore.Speak.config",
"\App_Config\Include\Sitecore.Speak.ItemWebApi.config",
"\App_Config\Include\Sitecore.Speak.LaunchPad.config",
"\App_Config\Include\Sitecore.Speak.Mvc.config",
"\App_Config\Include\Sitecore.WebDAV.config",
"\App_Config\Include\Sitecore.Xdb.Remote.Client.config.disabled",
"\App_Config\Include\Sitecore.Xdb.Remote.Client.MarketingAssets.config.disabled",
"\App_Config\Include\Sitecore.Xdb.Remote.Server.config.disabled",
"\App_Config\Include\Sitecore.Xdb.Remote.Server.MarketingAssets.config.disabled",
"\App_Config\Include\CES\Sitecore.CES.DeviceDetection.CheckInitialization.config.disabled",
"\App_Config\Include\ContentTesting\Sitecore.ContentTesting.Processing.Aggregation.config",
"\App_Config\Include\ExperienceAnalytics\Sitecore.ExperienceAnalytics.Aggregation.config",
"\App_Config\Include\ExperienceAnalytics\Sitecore.ExperienceAnalytics.Client.config",
"\App_Config\Include\ExperienceAnalytics\Sitecore.ExperienceAnalytics.Reduce.config",
"\App_Config\Include\ExperienceAnalytics\Sitecore.ExperienceAnalytics.StorageProviders.config",
"\App_Config\Include\ExperienceAnalytics\Sitecore.ExperienceAnalytics.WebAPI.config",
"\App_Config\Include\ExperienceProfile\Sitecore.ExperienceProfile.Client.config",
"\App_Config\Include\ExperienceProfile\Sitecore.ExperienceProfile.config",
"\App_Config\Include\ExperienceProfile\Sitecore.ExperienceProfile.Reporting.config",
"\App_Config\Include\FXM\Sitecore.FXM.Speak.config",
"\App_Config\Include\ListManagement\Sitecore.ListManagement.Client.config",
"\App_Config\Include\ListManagement\Sitecore.ListManagement.config",
"\App_Config\Include\ListManagement\Sitecore.ListManagement.Services.config",
"\App_Config\Include\Social\Sitecore.Social.ExperienceProfile.config",
"\App_Config\Include\Social\Sitecore.Social.Klout.config.disabled",
"\App_Config\Include\XdbCloud\Sitecore.Cloud.Xdb.config.disabled",
"\App_Config\Include\XdbCloud\Sitecore.ContentSearch.Cloud.DefaultIndexConfiguration.config.disabled",
"\App_Config\Include\XdbCloud\Sitecore.ContentSearch.Cloud.Index.Analytics.config.disabled",
"\sitecore\shell\Applications\Reports\Dashboard\CampaignCategoryDefaultSettings.config",
"\sitecore\shell\Applications\Reports\Dashboard\Configuration.config",
"\sitecore\shell\Applications\Reports\Dashboard\DefaultSettings.config",
"\sitecore\shell\Applications\Reports\Dashboard\SingleCampaignDefaultSettings.config",
"\sitecore\shell\Applications\Reports\Dashboard\SingleTrafficTypeDefaultSettings.config")

    return $files | % { Join-Path $webrootPath -ChildPath $_ }
}

function Get-LuceneFilesToDisableOnCDServer()
{
    $files = @("App_Config\Include\Sitecore.ContentSearch.Lucene.Index.Master.config",
			   "App_Config\Include\Sitecore.Marketing.Definitions.MarketingAssets.Repositories.Lucene.Index.Master.config",
			   "App_Config\Include\ListManagement\Sitecore.ListManagement.Lucene.Index.List.config",
			   "App_Config\Include\ListManagement\Sitecore.ListManagement.Lucene.IndexConfiguration.config",
               "App_Config\Include\Social\Sitecore.Social.Lucene.Index.Master.config")

    return $files | % { Join-Path $webrootPath -ChildPath $_ }
}

function Get-SolrFilesToDisableOnCDServer()
{
    $files = @("App_Config\Include\Sitecore.ContentSearch.Solr.Index.Master.config",
			   "App_Config\Include\Sitecore.Marketing.Definitions.MarketingAssets.Repositories.Solr.Index.Master.config",
			   "App_Config\Include\ListManagement\Sitecore.ListManagement.Solr.Index.List.config",
			   "App_Config\Include\ListManagement\Sitecore.ListManagement.Solr.IndexConfiguration.config",
               "App_Config\Include\Social\Sitecore.Social.Solr.Index.Master.config")

    return $files | % { Join-Path $webrootPath -ChildPath $_ }
}

function Get-FilesToEnableOnCDServer()
{
    $files = @("App_Config\Include\Sitecore.Analytics.Automation.TimeoutProcessing.config.disabled",
               "App_Config\Include\Sitecore.Analytics.Processing.Aggregation.Services.config.disabled",
               "App_Config\Include\Sitecore.Analytics.Processing.Services.config.disabled",
               "App_Config\Include\Sitecore.Analytics.Reporting.config.disabled",
               "App_Config\Include\Sitecore.Marketing.Client.config.disabled",
               "App_Config\Include\Sitecore.Processing.config.disabled",
			   "App_Config\Include\Sitecore.WebDAV.config.disabled",
               "App_Config\Include\Sitecore.PathAnalyzer.Client.config.disabled",
               "App_Config\Include\Sitecore.PathAnalyzer.config.disabled",
               "App_Config\Include\Sitecore.PathAnalyzer.Processing.config.disabled",
               "App_Config\Include\Sitecore.PathAnalyzer.Services.config.disabled",
               "App_Config\Include\Sitecore.PathAnalyzer.StorageProviders.config.disabled",
               "bin\Sitecore.PathAnalyzer.dll.disabled",
               "bin\Sitecore.PathAnalyzer.Client.dll.disabled",
               "bin\Sitecore.PathAnalyzer.Services.dll.disabled",
               "bin\Sitecore.SequenceAnalyzer.dll.disabled",
   			   "App_Config\Include\ContentTesting\Sitecore.ContentTesting.Processing.Aggregation.config.disabled",
               "App_Config\Include\ExperienceAnalytics\Sitecore.ExperienceAnalytics.Aggregation.config.disabled",
               "App_Config\Include\ExperienceAnalytics\Sitecore.ExperienceAnalytics.Client.config.disabled",
               "App_Config\Include\ExperienceAnalytics\Sitecore.ExperienceAnalytics.Reduce.config.disabled",
               "App_Config\Include\ExperienceAnalytics\Sitecore.ExperienceAnalytics.StorageProviders.config.disabled",
               "App_Config\Include\ExperienceAnalytics\Sitecore.ExperienceAnalytics.WebAPI.config.disabled",
               "bin\Sitecore.ExperienceAnalytics.dll.disabled",
               "bin\Sitecore.ExperienceAnalytics.Client.dll.disabled",
               "App_Config\Include\ExperienceProfile\Sitecore.ExperienceProfile.config.disabled",
               "App_Config\Include\ExperienceProfile\Sitecore.ExperienceProfile.Client.config.disabled"
               "App_Config\Include\ExperienceProfile\Sitecore.ExperienceProfile.Reporting.config.disabled",
               "App_Config\Include\FXM\Sitecore.FXM.Speak.config.disabled",
			   "App_Config\Include\ListManagement\Sitecore.ListManagement.Client.config.disabled",
			   "App_Config\Include\ListManagement\Sitecore.ListManagement.config.disabled",
			   "App_Config\Include\ListManagement\Sitecore.ListManagement.Services.config.disabled",
			   "App_Config\Include\Social\Sitecore.Social.ExperienceProfile.config.disabled")

    return $files | % { Join-Path $webrootPath -ChildPath $_ }
}

function Get-LuceneFilesToEnableOnCDServer()
{
    $files = @("App_Config\Include\Sitecore.ContentSearch.Lucene.Index.Master.config.disabled",
			   "App_Config\Include\Sitecore.Marketing.Definitions.MarketingAssets.Repositories.Lucene.Index.Master.config.disabled",
			   "App_Config\Include\ListManagement\Sitecore.ListManagement.Lucene.Index.List.config.disabled",
			   "App_Config\Include\ListManagement\Sitecore.ListManagement.Lucene.IndexConfiguration.config.disabled",
               "App_Config\Include\Social\Sitecore.Social.Lucene.Index.Master.config.disabled")

    return $files | % { Join-Path $webrootPath -ChildPath $_ }
}

function Get-SolrFilesToEnableOnCDServer()
{
    $files = @("App_Config\Include\Sitecore.ContentSearch.Solr.Index.Master.config.disabled",
			   "App_Config\Include\Sitecore.Marketing.Definitions.MarketingAssets.Repositories.Solr.Index.Master.config.disabled",
			   "App_Config\Include\ListManagement\Sitecore.ListManagement.Solr.Index.List.config.disabled",
			   "App_Config\Include\ListManagement\Sitecore.ListManagement.Solr.IndexConfiguration.config.disabled",
               "App_Config\Include\Social\Sitecore.Social.Solr.Index.Master.config.disabled")

    return $files | % { Join-Path $webrootPath -ChildPath $_ }
}

function ConfigureServerAsCD()
{
	# Disable config files
    Write-Host "Disabling files not needed on CD server." "White"
    foreach ($file in Get-FilesToDisableOnCDServer)
    {        
        if (Test-Path $file)
        {
            $fileName = Split-Path $file -leaf
            $newName = $fileName + ".disabled"
			
			$newFilePath = $file + ".disabled"
			if (Test-Path $newFilePath)
			{
				Remove-Item $newFilePath
				Write-Host $config "Deleted existing .disabled for: " $fileName -foregroundcolor white
			}
			
            Rename-Item -Path $file -NewName $newName
            Write-Host $config "Disabled: $file" -foregroundcolor white 
        }
        else
        {
            Write-Host "File not found on server: $file" -foregroundcolor red
        }
    }
	
	# Disable index files
	if ($IndexType -like 'L')
	{
		$indexFiles = Get-LuceneFilesToDisableOnCDServer
	}
	else 
	{
		$indexFiles = Get-SolrFilesToDisableOnCDServer
	}

	foreach ($file in $indexFiles)
    {        
        if (Test-Path $file)
        {
            $fileName = Split-Path $file -leaf
            $newName = $fileName + ".disabled"
			
			$newFilePath = $file + ".disabled"
			if (Test-Path $newFilePath)
			{
				Remove-Item $newFilePath
				Write-Host $config "Deleted existing .disabled for: " $fileName -foregroundcolor white
			}
			
            Rename-Item -Path $file -NewName $newName
            Write-Host $config "Disabled: $file" -foregroundcolor white 
        }
        else
        {
            Write-Host "File not found on server: $file" -foregroundcolor red
        }
    }	

	# Enable ScalabilitySettings.config
	$ScalabilitySettings = Join-Path $webrootPath -ChildPath "App_Config\Include\ScalabilitySettings.config.example"
	if (Test-Path $ScalabilitySettings) 
	{
		$switchFileName = Split-Path $ScalabilitySettings -leaf
		$switchNewName = $switchFileName -replace '.example',''
		Rename-Item -Path $ScalabilitySettings -NewName $switchNewName
		Write-Host $config "Enabled: $ScalabilitySettings" -foregroundcolor white 
	}
	else
	{
		$ScalabilitySettings = Join-Path $webrootPath -ChildPath "App_Config\Include\ScalabilitySettings.config.disabled"
		if (Test-Path $ScalabilitySettings)
		{
			$switchFileName = Split-Path $ScalabilitySettings -leaf
			$switchNewName = $switchFileName -replace '.disabled',''
			Rename-Item -Path $ScalabilitySettings -NewName $switchNewName
			Write-Host $config "Enabled: $ScalabilitySettings" -foregroundcolor white 
		}
		else
		{
			Write-Host $config "Could not find ScalabilitySettings.config.disabled or .example - " $ScalabilitySettings -foregroundcolor red			
		}		
	}	
	
	# Enable the SwitchMasterToWeb.config
	$SwitchMasterToWeb = Join-Path $webrootPath -ChildPath "App_Config\Include\zzzMustBeLast\SwitchMasterToWeb.config.disabled"
	if (Test-Path $SwitchMasterToWeb)
	{
		$switchFileName = Split-Path $SwitchMasterToWeb -leaf
		$switchNewName = $switchFileName -replace '.disabled',''
		Rename-Item -Path $SwitchMasterToWeb -NewName $switchNewName
		Write-Host $config "Enabled: $SwitchMasterToWeb" -foregroundcolor white 
	}
	else 
	{
		# check for zzzMustBeLast folder
		$MustBeLastFolder = Join-Path $webrootPath -ChildPath "App_Config\Include\zzzMustBeLast"
		if (-Not (Test-Path $MustBeLastFolder))
		{
			New-Item -ItemType directory -Path $MustBeLastFolder
			Write-Host $config "Folder \zzzMustBeLast created." -foregroundcolor white 
		}
		
		# check for SwitchMasterToWeb.config.example in \Include
		$SwitchMasterToWeb = Join-Path $webrootPath -ChildPath "App_Config\Include\zzzMustBeLast\SwitchMasterToWeb.config"
		$SwitchMasterToWebExample = Join-Path $webrootPath -ChildPath "App_Config\Include\SwitchMasterToWeb.config.example"
		if (Test-Path $SwitchMasterToWebExample)
		{
			# move it to zzzMustBeLast folder and enable it
			Move-Item $SwitchMasterToWebExample $SwitchMasterToWeb
			Write-Host $config "Enabled: $SwitchMasterToWeb." -foregroundcolor white 	
		}
		else
		{
			Write-Host $config "Could not find SwitchMasterToWeb.config - " $SwitchMasterToWeb -foregroundcolor red			
		}
	}
}

function ConfigureServerAsCM()
{
	# Enable config files
    Write-Host "Enabling files not needed on CD server." "White"
    foreach ($file in Get-FilesToEnableOnCDServer)
    {        
        if (Test-Path $file)
        {
            $fileName = Split-Path $file -leaf
            $newName = $fileName -replace '.disabled',''
            Rename-Item -Path $file -NewName $newName
            Write-Host $config "Enabled: $file" -foregroundcolor white 
        }
        else
        {
            Write-Host "File not found on server: $file" -foregroundcolor red
        }
    }
	
	# Enable index files
	if ($IndexType -like 'L')
	{
		$indexFiles = Get-LuceneFilesToEnableOnCDServer
	}
	else 
	{
		$indexFiles = Get-SolrFilesToEnableOnCDServer
	}
	
    foreach ($file in $indexFiles)
    {        
        if (Test-Path $file)
        {
            $fileName = Split-Path $file -leaf
            $newName = $fileName -replace '.disabled',''
            Rename-Item -Path $file -NewName $newName
            Write-Host $config "Enabled: $file" -foregroundcolor white 
        }
        else
        {
            Write-Host "File not found on server: $file" -foregroundcolor red
        }
    }
	
	# Disable ScalabilitySettings.config
	$ScalabilitySettings = Join-Path $webrootPath -ChildPath "App_Config\Include\ScalabilitySettings.config"
	if (Test-Path $ScalabilitySettings) 
	{
		$switchFileName = Split-Path $ScalabilitySettings -leaf
		$switchNewName = $switchFileName + ".disabled"
		Rename-Item -Path $ScalabilitySettings -NewName $switchNewName
		Write-Host $config "Enabled: $ScalabilitySettings" -foregroundcolor white 
	}
	else
	{
		Write-Host $config "Could not find ScalabilitySettings.config - " $ScalabilitySettings -foregroundcolor red			
	}	

	# Disable the SwitchMasterToWeb.config
	$SwitchMasterToWeb = Join-Path $webrootPath -ChildPath "App_Config\Include\zzzMustBeLast\SwitchMasterToWeb.config"
	if (Test-Path $SwitchMasterToWeb)
	{
		$switchFileName = Split-Path $SwitchMasterToWeb -leaf
		$switchNewName = $switchFileName + ".disabled"
		Rename-Item -Path $SwitchMasterToWeb -NewName $switchNewName
		Write-Host $config "Disabled: $SwitchMasterToWeb" -foregroundcolor white 
	}
	else 
	{
		# check for SwitchMasterToWeb.confg in \Include
		$SwitchMasterToWeb = Join-Path $webrootPath -ChildPath "App_Config\Include\SwitchMasterToWeb.config"
	
		if (Test-Path $SwitchMasterToWeb)
		{
			$switchFileName = Split-Path $SwitchMasterToWeb -leaf
			$switchNewName = $switchFileName + ".example"
			Rename-Item -Path $SwitchMasterToWeb -NewName $switchNewName
			Write-Host $config "Disabled: $SwitchMasterToWeb" -foregroundcolor white 		
		}
		else
		{
			Write-Host $config "Could not find SwitchMasterToWeb.config." $SwitchMasterToWeb -foregroundcolor red			
		}
	}
}

$webrootPath = Read-Host -Prompt 'Folder path the Sitecore instance \Website? (Ex: "D:\Sitecore\Dev-CD-SC8\Website\")'
$Command = Read-Host -Prompt 'Configure server to be [CM] or [CD]?'
$IndexType = Read-Host -Prompt 'Lucene [L] or Solr [S]?'
if ($Command -like 'CM')
{
	ConfigureServerAsCM
}
else
{
	ConfigureServerAsCD
}
