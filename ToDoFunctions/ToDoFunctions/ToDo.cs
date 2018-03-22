using Microsoft.WindowsAzure.Storage.Table;
using System;

public class ToDo : TableEntity
{
    public int UniqueID { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public DateTime? CompletedOn { get; set; }
    public DateTime? DeletedOn { get; set; }
}
