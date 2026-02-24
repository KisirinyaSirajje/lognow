using LogNow.API.DTOs;
using LogNow.API.Models;
using LogNow.API.Repositories;

namespace LogNow.API.Services;

public interface IWorkOrderCommentService
{
    Task<IEnumerable<WorkOrderCommentDto>> GetCommentsByWorkOrderIdAsync(Guid workOrderId);
    Task<WorkOrderCommentDto> AddCommentAsync(Guid workOrderId, CreateWorkOrderCommentDto createDto, Guid userId);
}

public class WorkOrderCommentService : IWorkOrderCommentService
{
    private readonly IWorkOrderCommentRepository _commentRepository;
    private readonly INotificationService _notificationService;
    private readonly IWorkOrderRepository _workOrderRepository;

    public WorkOrderCommentService(
        IWorkOrderCommentRepository commentRepository,
        INotificationService notificationService,
        IWorkOrderRepository workOrderRepository)
    {
        _commentRepository = commentRepository;
        _notificationService = notificationService;
        _workOrderRepository = workOrderRepository;
    }

    public async Task<IEnumerable<WorkOrderCommentDto>> GetCommentsByWorkOrderIdAsync(Guid workOrderId)
    {
        var comments = await _commentRepository.GetByWorkOrderIdAsync(workOrderId);
        return comments.Select(MapToDto);
    }

    public async Task<WorkOrderCommentDto> AddCommentAsync(Guid workOrderId, CreateWorkOrderCommentDto createDto, Guid userId)
    {
        var comment = new WorkOrderComment
        {
            Id = Guid.NewGuid(),
            WorkOrderId = workOrderId,
            UserId = userId,
            CommentText = createDto.CommentText,
            CreatedAt = DateTime.UtcNow
        };

        var createdComment = await _commentRepository.AddAsync(comment);

        // Create notification for assigned user
        var workOrder = await _workOrderRepository.GetByIdAsync(workOrderId);
        if (workOrder?.AssignedToUserId != null && workOrder.AssignedToUserId != userId)
        {
            await _notificationService.CreateNotificationAsync(
                workOrder.AssignedToUserId.Value,
                "Work Order Comment",
                $"New comment on work order {workOrder.WorkOrderNumber}",
                NotificationType.CommentAdded,
                workOrderId.ToString()
            );
        }

        return MapToDto(createdComment);
    }

    private static WorkOrderCommentDto MapToDto(WorkOrderComment comment)
    {
        return new WorkOrderCommentDto
        {
            Id = comment.Id,
            WorkOrderId = comment.WorkOrderId,
            UserId = comment.UserId,
            UserName = comment.User.FullName,
            CommentText = comment.CommentText,
            CreatedAt = comment.CreatedAt
        };
    }
}
