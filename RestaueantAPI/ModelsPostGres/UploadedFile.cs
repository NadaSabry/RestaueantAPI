using System;
using System.Collections.Generic;

namespace RestaueantAPI.ModelsPostGres;

public partial class UploadedFile
{
    public int Id { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? UserName { get; set; }

    public string? FileStatus { get; set; }

    public string? ErrorMsg { get; set; }

    public int? ServiceId { get; set; }

    public string? Filename { get; set; }

    public string? FileType { get; set; }

    public int? TotalCount { get; set; }

    public int? SuccessCount { get; set; }

    public int? FailedCount { get; set; }

    public double? SumAmount { get; set; }
}
