using System;
using System.Collections.Generic;

namespace DigitalCampusMap.Models;

public partial class Contactform
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
