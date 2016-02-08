using Newtonsoft.Json;
using Salesforce.Common.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Web.Models.Salesforce
{
    public class UserFeed
    {
        [Key]
        [Display(Name = "Feed Item ID")]
        [Createable(false), Updateable(false)]
        public string Id { get; set; }

        [Display(Name = "Parent ID")]
        [Createable(false), Updateable(false)]
        public string ParentId { get; set; }

        [Display(Name = "Feed Item Type")]
        [Createable(false), Updateable(false)]
        public string Type { get; set; }

        [Display(Name = "Created By ID")]
        [Createable(false), Updateable(false)]
        public string CreatedById { get; set; }

        [Display(Name = "Created Date")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset CreatedDate { get; set; }

        [Display(Name = "Deleted")]
        [Createable(false), Updateable(false)]
        public bool IsDeleted { get; set; }

        [Display(Name = "Last Modified Date")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset LastModifiedDate { get; set; }

        [Display(Name = "System Modstamp")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset SystemModstamp { get; set; }

        [Display(Name = "Comment Count")]
        [Createable(false), Updateable(false)]
        public int CommentCount { get; set; }

        [Display(Name = "Like Count")]
        [Createable(false), Updateable(false)]
        public int LikeCount { get; set; }

        [StringLength(255)]
        [Createable(false), Updateable(false)]
        public string Title { get; set; }

        [Createable(false), Updateable(false)]
        public string Body { get; set; }

        [Display(Name = "Link Url")]
        [Url]
        [Createable(false), Updateable(false)]
        public string LinkUrl { get; set; }

        [Display(Name = "Related Record ID")]
        [Createable(false), Updateable(false)]
        public string RelatedRecordId { get; set; }

        [Display(Name = "Content Data")]
        [Createable(false), Updateable(false)]
        public byte[] ContentData { get; set; }

        [Display(Name = "Content File Name")]
        [StringLength(255)]
        [Createable(false), Updateable(false)]
        public string ContentFileName { get; set; }

        [Display(Name = "Content Description")]
        [Createable(false), Updateable(false)]
        public string ContentDescription { get; set; }

        [Display(Name = "Content File Type")]
        [StringLength(120)]
        [Createable(false), Updateable(false)]
        public string ContentType { get; set; }

        [Display(Name = "Content Size")]
        [Createable(false), Updateable(false)]
        public int? ContentSize { get; set; }

        [Display(Name = "InsertedBy ID")]
        [Createable(false), Updateable(false)]
        public string InsertedById { get; set; }

    }
}
