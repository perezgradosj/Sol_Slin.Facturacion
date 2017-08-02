<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl" xmlns:fo="http://www.w3.org/1999/XSL/Format">
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="/">
    <fo:root xmlns:fo="http://www.w3.org/1999/XSL/Format">
      <fo:layout-master-set>
        <fo:simple-page-master master-name="first"
                               page-height="29.7cm"
                               page-width="21cm"
                               margin-top="1cm"
                               margin-bottom="1cm"
                               margin-left="2cm"
                               margin-right="2cm">
          <fo:region-body margin-top="3.5cm" margin-bottom="1.5cm"/>
          <fo:region-before extent="3cm"/>
          <fo:region-after extent="1.5cm"/>
        </fo:simple-page-master>
      </fo:layout-master-set>


      <fo:page-sequence master-reference="first">
        <fo:static-content  flow-name="xsl-region-before" >
          <fo:block font-weight="bold">          
          </fo:block>
        </fo:static-content>
        <fo:static-content  flow-name="xsl-region-after">
          <fo:block font-weight="bold">
          </fo:block>
        </fo:static-content>
        <fo:flow flow-name="xsl-region-body">
          <fo:block>
            <xsl:value-of select ="Agenda/LstPersonas/persona/Nombre" />
            <xsl:value-of select ="Agenda/LstPersonas/persona/Apellidos" />
            <xsl:value-of select ="Agenda/LstPersonas/persona/Mote" />
          </fo:block>
        </fo:flow>
      </fo:page-sequence>
      
    

    </fo:root>

    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
    </xsl:copy>
  </xsl:template>
</xsl:stylesheet>
