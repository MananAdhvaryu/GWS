using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MnM.GWS
{
    public interface ICryptoSerialize
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        string ID { get; }

        /// <summary>
        /// Gets or sets a value indicating whether [property modified].
        /// </summary>
        /// <value><c>true</c> if [property modified]; otherwise, <c>false</c>.</value>
        bool PropertyModified { get; set; }
        /// <summary>
        /// Writes the properties.
        /// </summary>
        void WriteProperties();
        /// <summary>
        /// Reads the properties.
        /// </summary>
        bool ReadProperties();
    }
    public interface IJSON
    {
        /// <summary>
        /// To the HTML.
        /// </summary>
        /// <returns>System.String.</returns>
        string ToJSON();
    }
}
