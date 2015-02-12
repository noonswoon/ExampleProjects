<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="BusServices" generation="1" functional="0" release="0" Id="c2df1fbb-3060-4b5a-a3e2-2fe8c473b38a" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="BusServicesGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="LogAPI:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/BusServices/BusServicesGroup/LB:LogAPI:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="LogAPIInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/BusServices/BusServicesGroup/MapLogAPIInstances" />
          </maps>
        </aCS>
        <aCS name="LogQueueService:Microsoft.ServiceBus.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/BusServices/BusServicesGroup/MapLogQueueService:Microsoft.ServiceBus.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="LogQueueServiceInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/BusServices/BusServicesGroup/MapLogQueueServiceInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:LogAPI:Endpoint1">
          <toPorts>
            <inPortMoniker name="/BusServices/BusServicesGroup/LogAPI/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapLogAPIInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/BusServices/BusServicesGroup/LogAPIInstances" />
          </setting>
        </map>
        <map name="MapLogQueueService:Microsoft.ServiceBus.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/BusServices/BusServicesGroup/LogQueueService/Microsoft.ServiceBus.ConnectionString" />
          </setting>
        </map>
        <map name="MapLogQueueServiceInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/BusServices/BusServicesGroup/LogQueueServiceInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="LogAPI" generation="1" functional="0" release="0" software="C:\Users\Vatthanachai\documents\visual studio 2012\Projects\BusServices\BusServices\csx\Debug\roles\LogAPI" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;LogAPI&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;LogAPI&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;LogQueueService&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/BusServices/BusServicesGroup/LogAPIInstances" />
            <sCSPolicyUpdateDomainMoniker name="/BusServices/BusServicesGroup/LogAPIUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/BusServices/BusServicesGroup/LogAPIFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="LogQueueService" generation="1" functional="0" release="0" software="C:\Users\Vatthanachai\documents\visual studio 2012\Projects\BusServices\BusServices\csx\Debug\roles\LogQueueService" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="Microsoft.ServiceBus.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;LogQueueService&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;LogAPI&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;LogQueueService&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/BusServices/BusServicesGroup/LogQueueServiceInstances" />
            <sCSPolicyUpdateDomainMoniker name="/BusServices/BusServicesGroup/LogQueueServiceUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/BusServices/BusServicesGroup/LogQueueServiceFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="LogAPIUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="LogQueueServiceUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="LogAPIFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="LogQueueServiceFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="LogAPIInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="LogQueueServiceInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="3041e4dd-f634-4e2c-89aa-d2a1df5ad626" ref="Microsoft.RedDog.Contract\ServiceContract\BusServicesContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="51ec2803-8b0e-4446-ba16-3e1594dc33c7" ref="Microsoft.RedDog.Contract\Interface\LogAPI:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/BusServices/BusServicesGroup/LogAPI:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>