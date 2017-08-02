using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Slin.Facturacion.DataAccess;
using System.Text;
using System.Xml;
using System.IO;

namespace Slin.Facturacion.Tests
{
    [TestClass]
    public class UnitTest1
    {
        //[TestMethod]
        //public void TestMethod1()
        //{
        //    Usuario oUsuario = new Usuario();
        //    oUsuario.CodUsuario = "123";
        //    oUsuario.Username = "";

        //    //var lista = new  UsuarioBusinessLogic().GetListaUsuario(oUsuario);


        //    //Assert.AreEqual(1, lista.Count);
        //}


        [TestMethod]
        public void TestMethod1()
        {
            string num_cpe = "20431084172-20-R001-000000046";

            string result = new ServiceConsultaDataAccess().GetDocumentoPDF(num_cpe);


            byte[] res = Convert.FromBase64String(result);
            string respuesta = Encoding.GetEncoding("iso-8859-1").GetString(res);

            System.IO.FileStream stream = new FileStream("D:/SLIN-ADE/Procesos/smc/forpdf/" + num_cpe +"_2.pdf", FileMode.CreateNew);
            System.IO.BinaryWriter writer =
                new BinaryWriter(stream);
            writer.Write(res, 0, res.Length);
            writer.Close();

        }


        [TestMethod]
        public void TestMethod2()
        {
            string num_cpe = "20431084172-20-R001-00000016";

            string result = new ServiceConsultaDataAccess().GetDocumentoXML(num_cpe);

            //byte[] res = Encoding.GetEncoding("").GetBytes(result);
            byte[] res = Convert.FromBase64String(result);

            string respuesta = Encoding.GetEncoding("iso-8859-1").GetString(res);


            var xmldoc = new XmlDocument();
            xmldoc.InnerXml = respuesta;
            xmldoc.Save("D:/SLIN-ADE/Procesos/smc/forpdf/" + num_cpe + ".xml");
        }


        [TestMethod]
        public void TestMethod3()
        {
            string num_cpe = "20431084172-20-R001-00000013";

            string result = new ServiceConsultaDataAccess().GetDocumentoXMLDir(num_cpe);


            byte[] res = Convert.FromBase64String(result);
            string respuesta = Encoding.GetEncoding("iso-8859-1").GetString(res);

            var xmldoc = new XmlDocument();
            xmldoc.InnerXml = respuesta;
            xmldoc.Save("D:/SLIN-ADE/Procesos/smc/forpdf/" + num_cpe + ".xml");

        }


        [TestMethod]
        public void TestMethod4()
        {
            string num_cpe = "20431084172-20-R001-00000046";

            string result = new ServiceConsultaDataAccess().GetDocumentoPDFDir(num_cpe);


            byte[] res = Convert.FromBase64String(result);
            string respuesta = Encoding.GetEncoding("iso-8859-1").GetString(res);

            var file = Path.Combine("D:/SLIN-ADE/Procesos/smc/forpdf/" + num_cpe + "_2.pdf");
            if (System.IO.File.Exists(file))
                System.IO.File.Delete(file);

            System.IO.FileStream stream = new FileStream("D:/SLIN-ADE/Procesos/smc/forpdf/" + num_cpe + "_2.pdf", FileMode.CreateNew);
            System.IO.BinaryWriter writer =
                new BinaryWriter(stream);
            writer.Write(res, 0, res.Length);
            writer.Close();

        }




        public void Encode(string inFileName, string outFileName)
        {
            System.Security.Cryptography.ICryptoTransform transform = new System.Security.Cryptography.ToBase64Transform();
            using (System.IO.FileStream inFile = System.IO.File.OpenRead(inFileName),
                                      outFile = System.IO.File.Create(outFileName))
            using (System.Security.Cryptography.CryptoStream cryptStream = new System.Security.Cryptography.CryptoStream(outFile, transform, System.Security.Cryptography.CryptoStreamMode.Write))
            {
                // I'm going to use a 4k buffer, tune this as needed
                byte[] buffer = new byte[4096];
                int bytesRead;

                while ((bytesRead = inFile.Read(buffer, 0, buffer.Length)) > 0)
                    cryptStream.Write(buffer, 0, bytesRead);

                cryptStream.FlushFinalBlock();
            }
        }

        public void Decode(string inFileName, string outFileName)
        {
            System.Security.Cryptography.ICryptoTransform transform = new System.Security.Cryptography.FromBase64Transform();
            using (System.IO.FileStream inFile = System.IO.File.OpenRead(inFileName),
                                      outFile = System.IO.File.Create(outFileName))
            using (System.Security.Cryptography.CryptoStream cryptStream = new System.Security.Cryptography.CryptoStream(inFile, transform, System.Security.Cryptography.CryptoStreamMode.Read))
            {
                byte[] buffer = new byte[4096];
                int bytesRead;

                while ((bytesRead = cryptStream.Read(buffer, 0, buffer.Length)) > 0)
                    outFile.Write(buffer, 0, bytesRead);

                outFile.Flush();
            }
        }

        // this version of Encode pulls everything into memory at once
        // you can compare the output of my Encode method above to the output of this one
        // the output should be identical, but the crytostream version
        // will use way less memory on a large file than this version.
        public void MemoryEncode(string inFileName, string outFileName)
        {
            byte[] bytes = System.IO.File.ReadAllBytes(inFileName);
            System.IO.File.WriteAllText(outFileName, System.Convert.ToBase64String(bytes));
        }
    }
}
