using api.Models.Complements;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class TransactionType
    {
        /// <summary>
        /// Represents the name of the transaction type. It is the primary key of the entity.
        /// </summary>
        [Key]
        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// This field indicates if the transaction type is simple or complex.
        /// <see langword="bool"/> is <c>true</c> if the transaction type is simple, <c>false</c> if it is complex.
        /// <list type="number">
        ///     <item>
        ///         Simple transaction types are those that do not require additional information/fields
        ///         than the <c>Transaction</c> base class.
        ///     </item>
        ///     <item>
        ///         Complex transaction types are those that require additional information/fields
        ///         the <c>Transaction</c> base class does not provide. Like transfers, interests transactions,
        ///         and so on.
        ///     </item>
        /// </list>
        /// </summary>
        public bool Simple { get; set; }

        /// <summary>
        /// <inheritdoc cref="Models.Complements.BalanceEffect"/>
        /// </summary>
        public BalanceEffect BalanceEffect { get; set; }

        /// <summary>
        /// The date and time when the transaction type was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// The date and time when the transaction type was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        
        public List<Transaction> Transactions { get; set; }
    }
}
