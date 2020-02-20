using System;
using System.Collections.Generic;

namespace MnM.GWS
{
#if VCSupport
    /// <summary>
    /// Object bound to data using IDataBoundElement
    /// </summary>
    public interface IDataBoundControl : IDataBoundElement
    {
        /// <summary>
        /// True indicate this object has fixed size.
        /// </summary>
        bool LockResizing { get; }
        /// <summary>
        /// Regresh from bound data source.
        /// </summary>
        void Refresh();
    }
    public interface IDataBoundElement
    {
        /// <summary>
        /// Gets the binder.
        /// </summary>
        /// <value>The binder.</value>
        IDataBinder Binder { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has binder.
        /// </summary>
        /// <value><c>true</c> if this instance has binder; otherwise, <c>false</c>.</value>
        bool HasBinder { get; }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <value>The field value.</value>
        string FieldValue { get; set; }
    }

    /// <summary>
    /// Object explaining the relation and sources of data.
    /// </summary>
    public interface IDataRelation : IJSON, ICloneable
    {
        /// <summary>
        /// Returns Usge enum for the data source.
        /// </summary>
        Usage Usage { get; set; }
        /// <summary>
        /// Gets or Sets a field in the data source.
        /// </summary>
        string DataField { get; set; }
        /// <summary>
        /// Gets or sets name of Table.
        /// </summary>
        string Table { get; set; }
        /// <summary>
        /// Get or set the Name of the Data Source!!!!
        /// </summary>
        string Text { get; set; }
        /// <summary>
        /// Gets or sets the increment for moving throught the datasource.!!!!
        /// </summary>
        long IncrementStep { get; set; }
        bool Unique { get; set; }
        /// <summary>
        /// Copies from specific Field in the data source or creted by the data relation.
        /// </summary>
        /// <param name="field"></param>
        void CopyFrom(IDataRelation field);
    }
    /// <summary>
    /// Object defining the relationship with the bound data.
    /// </summary>
    public interface IDataBinder : IDataRelation
    {
        /// <summary>
        /// Gets or sets a text description of the data.
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// Get or set the rule for Null handling
        /// </summary>
        bool AllowNull { get; set; }
        /// <summary>
        /// Gets a list of IDataRelation objects connected to data sources.
        /// </summary>
        IList<IDataRelation> Relations { get; }

        /// <summary>
        /// Get or sets the TypeCode enum of the object returned.
        /// </summary>
        TypeCode DataTypeCode { get; set; }
        /// <summary>
        /// Get or set the Data type of data read or written to the bound data object.
        /// </summary>
        Type DataType { get; set; }

        /// <summary>
        /// Copy data from the given source.
        /// </summary>
        /// <param name="source">Source of Data.</param>
        void CopyFrom(IDataBinder source);
    }
#endif
}
