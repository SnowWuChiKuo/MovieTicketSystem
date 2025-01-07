using ServerSide.Models.DAOs;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.Infra;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Services
{
    public class MemberService
    {
        private readonly MemberDao _dao;
        public MemberService(MemberDao dao)
        {
            _dao = dao;
        }

        public List<MemberIndexVm> GetAll()
        { 
            return _dao.GetAll();
        }
        public MemberDto Get(int id)
        {
            return _dao.Get(id);
           
        }

        public void Create(MemberDto model)
        {
            _dao.Create(model);
        }


        public MemberDto ConvertToDto(MemberCreateVm model)
        {
            var dto = new MemberDto
            {
                Account = model.Account,
                Email = model.Email,
                Password = model.Password,
                //PasswordHash = ,
                Name = model.Name,
                IsDeleted = model.IsDeleted,
                IsBlackList = model.IsBlackList,
                IsConfirmed = model.IsConfirmed,
                ConfirmCode = model.ConfirmCode,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };
            dto.PasswordHash = HashUtility.ToSHA256(model.Password, HashUtility.GetSalt());

            return dto;
        }

        public void Edit(MemberDto dto)
        {
            _dao.Edit(dto);
        }
    }
}
