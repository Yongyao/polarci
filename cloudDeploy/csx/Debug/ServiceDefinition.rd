<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="cloudDeploy" generation="1" functional="0" release="0" Id="ec68731a-f939-4aa6-9d91-5e1a301e0007" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="cloudDeployGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="GeoSearch.Web:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/cloudDeploy/cloudDeployGroup/LB:GeoSearch.Web:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="GeoSearch.Web:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/cloudDeploy/cloudDeployGroup/MapGeoSearch.Web:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="GeoSearch.WebInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/cloudDeploy/cloudDeployGroup/MapGeoSearch.WebInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:GeoSearch.Web:Endpoint1">
          <toPorts>
            <inPortMoniker name="/cloudDeploy/cloudDeployGroup/GeoSearch.Web/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapGeoSearch.Web:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/cloudDeploy/cloudDeployGroup/GeoSearch.Web/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapGeoSearch.WebInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/cloudDeploy/cloudDeployGroup/GeoSearch.WebInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="GeoSearch.Web" generation="1" functional="0" release="0" software="E:\Silverlight Development\Projects\GeoSearch-cisc-2012-07-23\cloudDeploy\csx\Debug\roles\GeoSearch.Web" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;GeoSearch.Web&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;GeoSearch.Web&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/cloudDeploy/cloudDeployGroup/GeoSearch.WebInstances" />
            <sCSPolicyUpdateDomainMoniker name="/cloudDeploy/cloudDeployGroup/GeoSearch.WebUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/cloudDeploy/cloudDeployGroup/GeoSearch.WebFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="GeoSearch.WebUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="GeoSearch.WebFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="GeoSearch.WebInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="a14f7983-bf0c-4e07-8ad5-22b099b07f7d" ref="Microsoft.RedDog.Contract\ServiceContract\cloudDeployContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="9fc0e641-2568-42d2-bffa-2664be58a5e2" ref="Microsoft.RedDog.Contract\Interface\GeoSearch.Web:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/cloudDeploy/cloudDeployGroup/GeoSearch.Web:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>