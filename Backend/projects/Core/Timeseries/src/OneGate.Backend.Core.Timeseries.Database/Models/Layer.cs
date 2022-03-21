﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneGate.Backend.Core.Timeseries.Database.Models
{
    [Table("layer")]
    public class Layer
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }
        
        [Required]
        [Column("owner_id")]
        public int OwnerId { get; set; }
        
        [Required]
        [Column("asset_id")]
        public int AssetId { get; set; }
        
        [Required]
        [Column("interval")]
        public string Interval { get; set; }

        [MaxLength(100)]
        [Column("name")]
        public string Name { get; set; }
    }
}