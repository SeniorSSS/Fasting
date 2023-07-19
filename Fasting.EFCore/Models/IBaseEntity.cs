namespace Fasting.EFCore.Models
{
    public interface IBaseEntity
    {
        /// <summary>
        /// Date when entity was created
        /// </summary>
        DateTime CreatedDate { get; set; }
    }
}
