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
  <xsl:output method="html" indent="yes"/>

  <xsl:template match="@* | node()">
    <!--<xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
    </xsl:copy>-->


    <html xmlns="http://www.w3.org/1999/xhtml">

      <head>

      </head>

      <body>

          
          <table>
              <tr>
                <td>
                  <img src="20100142041.jpg"/>

                </td>

              </tr>

            </table>
          
          <table>
              <tr>
                <td>
                  <xsl:value-of select="cac:AccountingSupplierParty/cbc:CustomerAssignedAccountID"/>

                </td>

              </tr>

            </table>
        

            <br></br>
            <table border="1">

              <tr>

                <table border="1">

                  <tr>
                    <td>
                      FACTURA ELECTRÓNICA
                    </td>
                  </tr>
                  <tr>
                    <td>
                      R.U.C.:
                      <xsl:value-of select="cac:AccountingSupplierParty/cbc:CustomerAssignedAccountID"/>
                    </td>
                  </tr>
                  <tr>
                  </tr>
                  <tr>
                    <td>

                      <xsl:value-of select="cbc:ID"/>
                    </td>
                  </tr>

                </table>

              </tr>




            </table>




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
