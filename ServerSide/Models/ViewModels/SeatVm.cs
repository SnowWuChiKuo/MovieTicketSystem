﻿using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models.ViewModels
{
    public class SeatVm
    {
        [Display(Name = "座位Id")]
        public int Id { get; set; }

        [Display(Name = "影廳名稱")]
        [Required]
        public int TheaterId { get; set; }
        
        [Display(Name = "座位行號")]
        [Required]
        [StringLength(2, ErrorMessage = "座位行號不可超過2個數字")]
        public string Row { get; set; }
        
        [Display(Name = "座位編號")]
        [Required]
		[StringLength(2, ErrorMessage = "座位行號不可超過2個數字")]
		public string Number { get; set; }

        [Display(Name = "無障礙座位狀態")]
        public bool IsDisabled { get; set; }

        // 新增的屬性
        [Display(Name = "無障礙座位狀態")]
        public string IsDisabledDisplay => IsDisabled ? "是" : "否";
    }
}
