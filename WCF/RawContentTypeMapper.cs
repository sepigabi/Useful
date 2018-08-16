namespace BusinessLogic
{
    using System.ServiceModel.Channels;

    public class RawContentTypeMapper : WebContentTypeMapper
    {
        public override WebContentFormat GetMessageFormatForContentType(string contentType)
        {
            return WebContentFormat.Raw;
        }
    }
}

//in the App.config:
//<service behaviorConfiguration="baseServerBehaviour" name="BusinessLogic.Services.SD_Service">
//  <endpoint address="" behaviorConfiguration="webBehaviour" binding="webHttpBinding" bindingConfiguration="HttpsBinding" contract="BusinessLogic.Services.ISD_Service" />
//  <endpoint address="mex" binding="mexHttpBinding" contract="IMetadataExchange" />
//  <host>
//    <baseAddresses>
//      <add baseAddress="https://localhost:8076/SD_Service" />
//          </baseAddresses>
//       </host>
//      </service>
//<bindings>
//  <webHttpBinding>
//    <binding name="HttpsBinding" closeTimeout="01:00:00" openTimeout="01:00:00" receiveTimeout="01:00:00" sendTimeout="01:00:00" maxReceivedMessageSize="2147483647" maxBufferSize="2147483647" contentTypeMapper="BusinessLogic.RawContentTypeMapper, BusinessLogic">
//      <security mode="Transport">
//        <transport clientCredentialType="Windows" proxyCredentialType="Windows">
//        </transport>
//      </security>
//    </binding>
//  </webHttpBinding>
//</bindings>
