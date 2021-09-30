using System;
using System.Security.Cryptography.X509Certificates;

namespace SG.Autorizador.Utilitarios
{
    /// <summary>
    /// Ayudante para interactuar con el almacén de certificados digitales.
    /// </summary>
    public class CertificateHelper : IDisposable
	{
		private X509Store _currentStore;

		/// <summary>
		/// <see cref="CertificateHelper"/>
		/// </summary>
		/// <param name="storeName"><see cref="StoreName"/></param>
		/// <param name="storeLocation"><see cref="StoreLocation"/></param>
		public CertificateHelper(StoreName storeName, StoreLocation storeLocation)
		{
			_currentStore = new X509Store(storeName, storeLocation);
			_currentStore.Open(OpenFlags.OpenExistingOnly);
		}


		/// <summary>
		/// Obtiene del almacén de certificados el certificado digital correspondiente con el subject suministrado.
		/// </summary>
		/// <param name="subject">Subject del certificado a buscar.</param>
		/// <returns><see cref="X509Certificate"/></returns>
		public X509Certificate GetCertificateBySubject(string subject)
		{
			X509CertificateCollection x509CertificateCollection = _currentStore.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, subject, false);
			X509Certificate result;
			if (x509CertificateCollection.Count > 0)
			{
				result = x509CertificateCollection[0];
			}
			else
			{
				result = null;
			}
			return result;
		}

		/// <summary>
		/// Obtiene del almacén de certificados el certificado digital correspondiente con el número de serie suministrado.
		/// </summary>
		/// <param name="serialNumber">Número de serie del certificado.</param>
		/// <returns><see cref="X509Certificate"/></returns>
		public X509Certificate2 GetCertificateBySerialNumber(string serialNumber)
		{
			X509Certificate2Collection x509Certificate2Collection = _currentStore.Certificates.Find(X509FindType.FindBySerialNumber, serialNumber, false);
			X509Certificate2 result;
			if (x509Certificate2Collection.Count > 0)
			{
				result = x509Certificate2Collection[0];
			}
			else
			{
				result = null;
			}
			return result;
		}

		/// <summary>
		/// Obtiene del almacén de certificados el certificado digital correspondiente con el nombre del emisor.
		/// </summary>
		/// <param name="pIssuerName">Nombre del emisor.</param>
		/// <returns><see cref="X509Certificate"/></returns>
		public X509Certificate GetCertificateByIssuerName(string pIssuerName)
		{
			X509CertificateCollection x509CertificateCollection = _currentStore.Certificates.Find(X509FindType.FindByIssuerName, pIssuerName, true);
			X509Certificate result;
			if (x509CertificateCollection.Count > 0)
			{
				result = x509CertificateCollection[0];
			}
			else
			{
				result = null;
			}
			return result;
		}

		/// <summary>
		/// Libera los recursos de la instancia.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Libera los recursos de la instacia.
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			_currentStore.Close();
			_currentStore.Dispose();
			_currentStore = null;
		}
	}
}