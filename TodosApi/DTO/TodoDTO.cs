using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace TodosApi.DTO
{
    public class TodoDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public string CreatedDate { get; set; }
        public string DueDate { get; set; }
        public string Priority { get; set; }
    }



    public class TodoDTOValidator : AbstractValidator<TodoDTO>
    {
        public TodoDTOValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Title).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
            RuleFor(x => x.CreatedDate).NotEmpty().WithMessage("CreatedDate is required");
            RuleFor(x => x.DueDate).NotEmpty().WithMessage("DueDate is required");
            RuleFor(x => x.Priority).NotEmpty().WithMessage("Priority is required");
        }
    }
}