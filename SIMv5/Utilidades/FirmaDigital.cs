namespace SIM.Utilidades.FirmaDigital
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web;
    using co.com.certicamara.encryption3DES.code;
    using O2S.Components.PDF4NET.PDFFile;
    using O2S.Components.PDF4NET.Text;

    public class Comun
    {
        HttpClient client = new HttpClient();

        public Comun()
        {
            var _Licencia = Data.ObtenerValorParametro("LicenciaMicroSign");
            if (UrlWebSerice != null && UrlWebSerice.Length <= 0)
            {
                throw new Exception("No se ha configurado la direccion URL del servicio web para la firma digital");
            }
            client.BaseAddress = new Uri(UrlWebSerice);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-api-key", _Licencia);
        }

        string RutaDocFirmado = Data.ObtenerValorParametro("RutaDocumentosFirmaDigital").ToString();
        string UrlWebSerice = Data.ObtenerValorParametro("UrlFirmaDocumentos").ToString();

        private async Task<ResponseDTO> ListCertificateAsync(ListCertificateDTO listCertificateDTO)
        {
            ResponseDTO responseDTO = null;
            //  client.CancelPendingRequests();
            HttpResponseMessage response = client.PostAsJsonAsync("listCertificate", listCertificateDTO).Result;
            responseDTO = await response.Content.ReadAsAsync<ResponseDTO>();
            return responseDTO;
        }

        private async Task<ResponseDTO> SignAsync(SignDTO signDTO)
        {
            ResponseDTO responseDTO = null;
            HttpResponseMessage response = client.PostAsJsonAsync("sign", signDTO).Result;
            responseDTO = await response.Content.ReadAsAsync<ResponseDTO>();
            //string data = await response.Content.ReadAsStringAsync();
            return responseDTO;
        }

        /// <summary>
        /// Recibe un arreglo de bytes con el documento y lo firma con el usuario y la contraseña aportados 
        /// </summary>
        /// <param name="_Usuario">Usuario que firma</param>
        /// <param name="_Password">Contraseña del usuario que firma</param>
        /// <param name="_Archivo">Arreglo de bytes con el documento a firmar</param>
        /// <returns></returns>
        public async Task<RespuestaFirma> FirmaDocumento(string _Usuario, string _Password, byte[] _Archivo)
        {
            RespuestaFirma _respuesta = new RespuestaFirma();
            if (_Usuario.Length <= 0)
            {
                _respuesta.Exito = false;
                _respuesta.Mensaje = "No se ingreso el usuario firmante";
                return _respuesta;
            }
            if (_Password.Length <= 0)
            {
                _respuesta.Exito = false;
                _respuesta.Mensaje = "No se ingreso la contraseña del usuario firmante";
                return _respuesta;
            }
            if (_Archivo.Length <= 0)
            {
                _respuesta.Exito = false;
                _respuesta.Mensaje = "El documento a firmar se encuentra vacio";
                return _respuesta;
            }
            var listCertificateDTO = new ListCertificateDTO()
            {
                nuip = _Usuario,
                password = _Password
            };
            try
            {
                var responseListDTO = await ListCertificateAsync(listCertificateDTO);
                foreach (ServiceErrorDTO foo in responseListDTO.errores)
                {
                    _respuesta.Exito = false;
                    _respuesta.Mensaje = "Error: Codigo: " + foo.code.ToString() + ", Mensaje: " + foo.message;
                    return _respuesta;
                }
                string _FileInibase64 = Convert.ToBase64String(_Archivo);
                string _RutaFirmado = RutaDocFirmado;
                if (_RutaFirmado != "")
                {
                    _RutaFirmado += @"\" + DateTime.Now.Year.ToString() + @"\" + DateTime.Now.Month.ToString("00");
                    if (!Directory.Exists(_RutaFirmado)) Directory.CreateDirectory(_RutaFirmado);
                    string _ArchFirmado = Guid.NewGuid().ToString() + ".pdf";
                    Encrypt3DES ed3des = new Encrypt3DES();
                    var signDTO = new SignDTO()
                    {
                        signType = "PADES",
                        signReason_idXml = "Razón de Firma= Validez Jurídica",
                        signLocation = "Lugar de Firma= Medellin, Colombia",
                        fileToSignBytes = _FileInibase64,
                        signedPath = _RutaFirmado,
                        fileSigned = _ArchFirmado,
                        certificateParameters = new CertificateParametersDTO()
                        {
                            certitoken = true,
                            user = _Usuario,
                            password = ed3des.encrypt(_Password),
                            serialCert = responseListDTO.listCertificateResponse[0].serial,
                            issuerCert = responseListDTO.listCertificateResponse[0].issuer
                        }
                    };
                    var responseSigntDTO = await SignAsync(signDTO);
                    if (responseSigntDTO.exitoso)
                    {
                        if (File.Exists(_RutaFirmado + @"\" + _ArchFirmado))
                        {
                            _respuesta.Exito = true;
                            byte[] _ArchivoFirmado = File.ReadAllBytes(_RutaFirmado + @"\" + _ArchFirmado);
                            _respuesta.ArchivoFirmado = _ArchivoFirmado;
                        }
                        else
                        {
                            _respuesta.Exito = false;
                            _respuesta.Mensaje = "Ocurrio un problema con ela archivo firmado!";
                        }
                    }
                    else
                    {
                        _respuesta.Exito = false;
                        _respuesta.Mensaje = responseSigntDTO.errores[0].code + ", " + responseSigntDTO.errores[0].message;
                    }
                }
                else
                {
                    _respuesta.Exito = false;
                    _respuesta.Mensaje = "No se ha configirado la ruta para los documentos firmados";
                }
            }
            catch (Exception ex)
            {
                _respuesta.Exito = false;
                _respuesta.Mensaje = "Ocurrió un error: " + ex.Message;
            }
            return _respuesta;
        }
        /// <summary>
        /// Recibe la ruta del documento y lo firma con el usuario y la contraseña aportados 
        /// </summary>
        /// <param name="_Usuario">Usuario que firma</param>
        /// <param name="_Password">Contraseña del usuario que firma</param>
        /// <param name="_Archivo">ruta del documento a firmar</param>
        /// <returns></returns>
        public async Task<RespuestaFirma> FirmaDocumento(string _Usuario, string _Password, string _Archivo)
        {
            RespuestaFirma _respuesta = new RespuestaFirma();
            byte[] _ArchBytes = null;
            if (!File.Exists(_Archivo))
            {
                _respuesta.Exito = false;
                _respuesta.Mensaje = "El documento a firmar no existe en la ruta especificada";
                return _respuesta;
            }
            else
            {
                _ArchBytes = File.ReadAllBytes(_Archivo);
            }
            if (_Usuario.Length <= 0)
            {
                _respuesta.Exito = false;
                _respuesta.Mensaje = "No se ingreso el usuario firmante";
                return _respuesta;
            }
            if (_Password.Length <= 0)
            {
                _respuesta.Exito = false;
                _respuesta.Mensaje = "No se ingreso la contraseña del usuario firmante";
                return _respuesta;
            }
            try
            {
                _respuesta = await FirmaDocumento(_Usuario, _Password, _ArchBytes);
            }
            catch (Exception ex)
            {
                _respuesta.Exito = false;
                _respuesta.Mensaje = "Ocurrió un error: " + ex.Message;
            }
            return _respuesta;
        }
        /// <summary>
        /// Recibe un arreglo de bytes con el documento y lo firma con el usuario y la contraseña aportados 
        /// </summary>
        /// <param name="_Usuario">Usuario que firma</param>
        /// <param name="_Password">Contraseña del usuario que firma</param>
        /// <param name="_Archivo">Arreglo de bytes con el documento a firmar</param>
        /// <returns></returns>
        public async Task<RespuestaFirma> FirmaDocumentoBytes(string _Usuario, string _Password, byte[] _Archivo, byte[] _Firma, CoordinatesDTO Pos, int PagFirma)
        {
            RespuestaFirma _respuesta = new RespuestaFirma();
            if (_Usuario.Length <= 0)
            {
                _respuesta.Exito = false;
                _respuesta.Mensaje = "No se ingreso el usuario firmante";
                return _respuesta;
            }
            if (_Password.Length <= 0)
            {
                _respuesta.Exito = false;
                _respuesta.Mensaje = "No se ingreso la contraseña del usuario firmante";
                return _respuesta;
            }
            if (_Archivo.Length <= 0)
            {
                _respuesta.Exito = false;
                _respuesta.Mensaje = "El documento a firmar se encuentra vacio";
                return _respuesta;
            }
            var listCertificateDTO = new ListCertificateDTO()
            {
                nuip = _Usuario,
                password = _Password
            };
            try
            {
                var responseListDTO = await ListCertificateAsync(listCertificateDTO);
                foreach (ServiceErrorDTO foo in responseListDTO.errores)
                {
                    _respuesta.Exito = false;
                    _respuesta.Mensaje = "Error: Codigo: " + foo.code.ToString() + ", Mensaje: " + foo.message;
                    return _respuesta;
                }
                string _FileInibase64 = Convert.ToBase64String(_Archivo);
                string _FirmaBase64 = Convert.ToBase64String(_Firma);
                Encrypt3DES ed3des = new Encrypt3DES();
                var signDTO = new SignDTO()
                {
                    signType = "PADES",
                    signReason_idXml = "Razón de Firma= Validez Jurídica",
                    signLocation = "Lugar de Firma= Medellín, Colombia",
                    fileToSignBytes = _FileInibase64,
                    visibleSign = true,
                    stamp = true,
                    ltv = true,
                    signByParts = false,
                    imageParameters = new ImageParametersDTO
                    {
                        imageBytes = _FirmaBase64,
                        coordinates = new CoordinatesDTO
                        {
                            lowerLeftX = Pos.lowerLeftX,
                            lowerLeftY = Pos.lowerLeftY,
                            upperRightX = Pos.upperRightX,
                            upperRightY = Pos.upperRightY
                        },
                        contentSignature = "FIRMA DIGITAL",
                        renderingMode = "GRAPHIC",
                        signFieldName = "FIRMA_1",
                        numPages = PagFirma.ToString(),
                        imageValidation = true
                    },
                    tsaParameters = new TsaParametersDTO()
                    {
                        userTSA = "CE20190906402",
                        passwordTSA = "CertiAmva202",
                        stampType = "user"
                    },
                    certificateParameters = new CertificateParametersDTO()
                    {
                        certitoken = true,
                        user = _Usuario,
                        password = ed3des.encrypt(_Password),
                        serialCert = responseListDTO.listCertificateResponse[0].serial,
                        issuerCert = responseListDTO.listCertificateResponse[0].issuer
                    }
                };
                var responseSigntDTO = await SignAsync(signDTO);
                if (responseSigntDTO.exitoso)
                {
                    if (responseSigntDTO.signResponse.signedBytes.Length > 0)
                    {
                        byte[] _ArchResp = Convert.FromBase64String(responseSigntDTO.signResponse.signedBytes);
                        _respuesta.Exito = true;
                        _respuesta.ArchivoFirmado = _ArchResp;
                    }
                    else
                    {
                        _respuesta.Exito = false;
                        _respuesta.Mensaje = "Ocurrio un problema con el archivo firmado!";
                    }
                }
                else
                {
                    _respuesta.Exito = false;
                    _respuesta.Mensaje = responseSigntDTO.errores[0].code + ", " + responseSigntDTO.errores[0].message;
                }
            }
            catch (Exception ex)
            {
                _respuesta.Exito = false;
                _respuesta.Mensaje = "Ocurrió un error: " + ex.Message;
            }
            return _respuesta;
        }
        /// <summary>
        /// Recibe un arreglo de bytes con el documento y lo firma con el usuario y la contraseña aportados 
        /// </summary>
        /// <param name="_Usuario">Usuario que firma</param>
        /// <param name="_Password">Contraseña del usuario que firma</param>
        /// <param name="_Archivo">Arreglo de bytes con el documento a firmar</param>
        /// <returns></returns>
        public async Task<RespuestaFirma> FirmaDocumentoBytes(string _Usuario, string _Password, byte[] _Archivo)
        {
            RespuestaFirma _respuesta = new RespuestaFirma();
            if (_Usuario.Length <= 0)
            {
                _respuesta.Exito = false;
                _respuesta.Mensaje = "No se ingreso el usuario firmante";
                return _respuesta;
            }
            if (_Password.Length <= 0)
            {
                _respuesta.Exito = false;
                _respuesta.Mensaje = "No se ingreso la contraseña del usuario firmante";
                return _respuesta;
            }
            if (_Archivo.Length <= 0)
            {
                _respuesta.Exito = false;
                _respuesta.Mensaje = "El documento a firmar se encuentra vacio";
                return _respuesta;
            }
            var listCertificateDTO = new ListCertificateDTO()
            {
                nuip = _Usuario,
                password = _Password
            };
            try
            {
                var responseListDTO = await ListCertificateAsync(listCertificateDTO);
                foreach (ServiceErrorDTO foo in responseListDTO.errores)
                {
                    _respuesta.Exito = false;
                    _respuesta.Mensaje = "Error: Codigo: " + foo.code.ToString() + ", Mensaje: " + foo.message;
                    return _respuesta;
                }
                string _FileInibase64 = Convert.ToBase64String(_Archivo);
                Encrypt3DES ed3des = new Encrypt3DES();
                var signDTO = new SignDTO()
                {
                    signType = "PADES",
                    signReason_idXml = "Razón de Firma= Validez Jurídica",
                    signLocation = "Lugar de Firma= Medellin, Colombia",
                    fileToSignBytes = _FileInibase64,
                    visibleSign = false,
                    stamp = true,
                    ltv = true,
                    tsaParameters = new TsaParametersDTO()
                    {
                        userTSA = "CE20190906402",
                        passwordTSA = "CertiAmva202",
                        stampType = "user"
                    },
                    certificateParameters = new CertificateParametersDTO()
                    {
                        certitoken = true,
                        user = _Usuario,
                        password = ed3des.encrypt(_Password),
                        serialCert = responseListDTO.listCertificateResponse[0].serial,
                        issuerCert = responseListDTO.listCertificateResponse[0].issuer
                    }
                };
                var responseSigntDTO = await SignAsync(signDTO);
                if (responseSigntDTO.exitoso)
                {
                    if (responseSigntDTO.signResponse.signedBytes.Length > 0)
                    {
                        byte[] _ArchResp = Convert.FromBase64String(responseSigntDTO.signResponse.signedBytes);
                        _respuesta.Exito = true;
                        _respuesta.ArchivoFirmado = _ArchResp;
                    }
                    else
                    {
                        _respuesta.Exito = false;
                        _respuesta.Mensaje = "Ocurrio un problema con el archivo firmado!";
                    }
                }
                else
                {
                    _respuesta.Exito = false;
                    _respuesta.Mensaje = responseSigntDTO.errores[0].code + ", " + responseSigntDTO.errores[0].message;
                }
            }
            catch (Exception ex)
            {
                _respuesta.Exito = false;
                _respuesta.Mensaje = "Ocurrió un error: " + ex.Message;
            }
            return _respuesta;
        }
        /// <summary>
        /// Metodo para validar si el suaurio y contraseña son validos para firmar un documento
        /// </summary>
        /// <param name="_Usuario">Usuario (login) del firmante</param>
        /// <param name="_Password">Contraseña del usuario que firma del documento</param>
        /// <returns></returns>
        public async Task<Valida> ValidaUsuario(string _Usuario, string _Password)
        {

            Valida _Rpta = new Valida();
            if (_Usuario.Length <= 0)
            {
                _Rpta.Exito = false;
                _Rpta.Mensaje = "No se ingreso el usuario firmante";
                return _Rpta;
            }
            if (_Password.Length <= 0)
            {
                _Rpta.Exito = false;
                _Rpta.Mensaje = "No se ingreso la contraseña del usuario firmante";
                return _Rpta;
            }

            var listCertificateDTO = new ListCertificateDTO()
            {
                nuip = _Usuario,
                password = _Password
            };
            try
            {
                _Rpta.Exito = true;
                _Rpta.Mensaje = "";
                var responseListDTO = await ListCertificateAsync(listCertificateDTO);
                foreach (ServiceErrorDTO foo in responseListDTO.errores)
                {
                    _Rpta.Exito = false;
                    _Rpta.Mensaje = "Error: Codigo: " + foo.code.ToString() + ", Mensaje: " + foo.message;
                    return _Rpta;
                }
            }
            catch (Exception ex)
            {
                _Rpta.Exito = false;
                _Rpta.Mensaje = "Ocurrió un error: " + ex.Message;
            }
            return _Rpta;
        }
    }

    public class FirmaProyeccion
    {
        public static async Task<RespuestaFirma> FirmaDocumento(byte[] _Documento,  byte[] _Firma, int PagFirma, string campoFirma, string Usuario, string Password)
        {
            RespuestaFirma _respuesta = new RespuestaFirma();
            Comun Firma = new Comun();
            Encrypt3DES ed3des = new Encrypt3DES();
            if (Usuario.Length <= 0)
            {
                _respuesta.Exito = false;
                _respuesta.Mensaje = "No se ha ingresado el usuario que firma el documento";
                return _respuesta;
            }
            if (Password.Length <= 0)
            {
                _respuesta.Exito = false;
                _respuesta.Mensaje = "No se ha ingresado la contraseña de la firma digital del usuario que firma el documento";
                return _respuesta;
            }
            if (_Documento.Length <= 0)
            {
                _respuesta.Exito = false;
                _respuesta.Mensaje = "El documento a firmar se encuentra vacio";
                return _respuesta;
            }
            if (campoFirma.Length <= 0)
            {
                _respuesta.Exito = false;
                _respuesta.Mensaje = "No se ha ingresado el campo sobre el cual va el grafico de la firma digital";
                return _respuesta;
            }
            PDFFile _docPdf = PDFFile.FromStream(new MemoryStream(_Documento));
            PDFTextRun _TxtFirma = null;
            PDFImportedPage ip = _docPdf.ExtractPage(PagFirma - 1);
            PDFSearchTextResultCollection _result = ip.SearchText(campoFirma);
            if (_result != null && _result.Count > 0)
            {
                _TxtFirma = _result[0].TextRuns[0];
            }
            string UsuarioFirma = Usuario;
            int _rect0 = Convert.ToInt32(Math.Round(_TxtFirma.DisplayBounds.Left)) - 2;
            int _rect1 = Convert.ToInt32(ip.Height - (Math.Round(_TxtFirma.DisplayBounds.Top)) + 2);
            int _rect2 = Convert.ToInt32((Math.Round(_TxtFirma.DisplayBounds.Left)) + 240);
            int _rect3 = Convert.ToInt32(ip.Height - (Math.Round(_TxtFirma.DisplayBounds.Top)) - 78);
            CoordinatesDTO _Posicion = new CoordinatesDTO { lowerLeftX = _rect0, lowerLeftY = _rect1, upperRightX = _rect2, upperRightY = _rect3 };
            _respuesta = await Firma.FirmaDocumentoBytes(UsuarioFirma, Password, _Documento, _Firma, _Posicion, PagFirma);
            if (_respuesta.Exito)
            {
                _respuesta.Exito = true;
                _respuesta.ArchivoFirmado = _respuesta.ArchivoFirmado;
            }
            else
            {
                _respuesta.Exito = false;
                _respuesta.Mensaje = _respuesta.Mensaje;
            }
            return _respuesta;
        }
    }

    #region Modelos de datos
    #region ListCertificate
    public class ListCertificateDTO
    {
        public string nuip { get; set; }
        public string password { get; set; }
    }

    #endregion

    #region Response
    public class ListCertificateResponseDTO
    {
        public string cn { get; set; }
        public string o { get; set; }
        public string serialNumber { get; set; }
        public string ou { get; set; }
        public string serial { get; set; }
        public string issuer { get; set; }
        public ValidityResponseDTO validity { get; set; }
    }

    public class ResponseDTO
    {
        public List<ServiceErrorDTO> errores { get; set; }
        public bool exitoso { get; set; }
        public SignResponseDTO signResponse { get; set; }
        public List<ListCertificateResponseDTO> listCertificateResponse { get; set; }

    }

    public class ServiceErrorDTO
    {
        public string code { get; set; }
        public string message { get; set; }
    }

    public class SignResponseDTO
    {
        public string signedPath { get; set; }
        public string signedBytes { get; set; }
        public DateTime? timeStamp { get; set; }
        public bool isTimeStamped { get; set; }
        public long timeStampSeconds { get; set; }
    }

    public class ValidityResponseDTO
    {
        public string notBefore { get; set; }
        public string notAfter { get; set; }
    }
    #endregion

    #region Sign

    public class CertificateParametersDTO
    {
        public string user { get; set; }
        public string certificatePath { get; set; }
        public string certificateBytes { get; set; }
        public string password { get; set; }
        public string serialCert { get; set; }
        public string issuerCert { get; set; }
        public bool certitoken { get; set; }
    }

    public class CoordinatesDTO
    {
        public int lowerLeftX { get; set; }
        public int lowerLeftY { get; set; }
        public int upperRightX { get; set; }
        public int upperRightY { get; set; }
    }

    public class ElectronicSignatureParametersDTO
    {
        public string type { get; set; }
        public string arguments { get; set; }
        public string electronicData { get; set; }
        public bool cypher { get; set; }
        public string cerPath { get; set; }
        public string cerBytes { get; set; }
        public string cer { get; set; }
    }

    public class ImageParametersDTO
    {
        public string imagePath { get; set; }
        public string imageBytes { get; set; }
        public string signFieldName { get; set; }
        public string stringToFind { get; set; }
        public string contentSignature { get; set; }
        public string numPages { get; set; }
        public string renderingMode { get; set; }
        public bool imageValidation { get; set; }
        public CoordinatesDTO coordinates { get; set; }
    }

    public class PolicyParametersDTO
    {
        public string sigPolicyId { get; set; }
        public string sigPolicyLocation { get; set; }
        public byte[] sigPolicyBytes { get; set; }
    }

    public class SignDTO
    {
        public string signType { get; set; }
        public string signReason_idXml { get; set; }
        public string signLocation { get; set; }
        public bool visibleSign { get; set; }
        public bool stamp { get; set; }
        public bool ltv { get; set; }
        public bool signByParts { get; set; }
        public bool verifyDocument { get; set; }
        public bool electronicSignature { get; set; }
        public bool apostille { get; set; }
        public bool noallowCopy { get; set; }
        public string fileToSignPath { get; set; }
        public string fileToSignBytes { get; set; }
        public string signedPath { get; set; }
        public string fileSigned { get; set; }
        public int pdfSignTypeConstants { get; set; }
        public Dictionary<string, string> apostilleProperties { get; set; }
        public ImageParametersDTO imageParameters { get; set; }
        public CertificateParametersDTO certificateParameters { get; set; }
        public TsaParametersDTO tsaParameters { get; set; }
        public ElectronicSignatureParametersDTO electronicSignatureParameters { get; set; }
        public PolicyParametersDTO policyParameters { get; set; }
        public string signerRole { get; set; }
        public string signatureLocation { get; set; }
    }

    public class TsaParametersDTO
    {
        public string userTSA { get; set; }
        public string certificateTSAPath { get; set; }
        public string certificateTSABytes { get; set; }
        public string passwordTSA { get; set; }
        public string stampType { get; set; }
    }
    #endregion

    #region Respuesta 
    public class RespuestaFirma
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public byte[] ArchivoFirmado { get; set; }
        public string RutaArchivoFirmado { get; set; }
    }
    public class Valida
   {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
    }
    #endregion
    #endregion
}