#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models;

public class RSVP
{
    [Key]
    public int RSVPId { get; set; }

    /**********************************************************************
Relationship properties below

Foreign Keys: id of a different (foreign) model

Navigation props:
    Data type is a related model
    MUST use .Inlcude for the nav prop data to be included via a SQL JOIN.

***************************************************************************/

    public int UserId { get; set; } //this foreign HAS TO MACH the PK property name
    public int WeddingId { get; set; } //this foreign HAS TO MACH the PK property name
    public User? User { get; set; }
    public Wedding? Wedding { get; set; }

}