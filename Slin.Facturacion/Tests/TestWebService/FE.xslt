<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
                
    xmlns:ds="http://www.w3.org/2000/09/xmldsig#"
    xmlns:qdt="urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2"
    xmlns:cbc="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"
    xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
    xmlns:udt="urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2"
    xmlns:cac="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2"
    xmlns:ext="urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2"
    xmlns:sac="urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1"
    xmlns:ccts="urn:un:unece:uncefact:documentation:2"
    xmlns="urn:oasis:names:specification:ubl:schema:xsd:Invoice-2"
  
  
>
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
    </xsl:copy>


    <html>

      <body>


        <div class="row">
          <div class="col-md-3">


          </div>
          <div class="col-md-3">


          </div>
          <div class="col-md-3">


          </div>
          <div class="col-md-3">
            <table border="1">
              <tr>
                <td> RUC: 
                  <xsl:value-of select="cac:AccountingSupplierParty/cbc:CustomerAssignedAccountID"/>
                </td>
              </tr>

              <tr>
                <td>
                  <xsl:value-of select="cac:AccountingSupplierParty/cbc:Party/cac:PartyName/cbc:Name"/>
                </td>
              </tr>

              <tr>
                <td>
                </td>
              </tr>



            </table>
          </div>


        </div>



        <table>
          <tr>
            <td colspan="3">
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
          </tr>

        </table>

      </body>
    </html>


  </xsl:template>
</xsl:stylesheet>
