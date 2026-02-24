import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { workOrderService } from '../services/workOrderService';
import { workOrderNoteService } from '../services/workOrderNoteService';
import { userService } from '../services/userService';
import { WorkOrder, WorkOrderNote, User, WorkOrderStatus } from '../types';

const WorkOrderDetailsPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [workOrder, setWorkOrder] = useState<WorkOrder | null>(null);
  const [notes, setNotes] = useState<WorkOrderNote[]>([]);
  const [newNote, setNewNote] = useState('');
  const [loading, setLoading] = useState(true);
  const [submittingNote, setSubmittingNote] = useState(false);
  const [teams, setTeams] = useState<string[]>([]);
  const [selectedGroup, setSelectedGroup] = useState<string>('');
  const [groupUsers, setGroupUsers] = useState<User[]>([]);
  const [selectedUserId, setSelectedUserId] = useState<string>('');
  const [assigning, setAssigning] = useState(false);
  const [updatingStatus, setUpdatingStatus] = useState(false);

  useEffect(() => {
    if (id) {
      loadWorkOrder();
      loadNotes();
      loadTeams();
    }
  }, [id]);

  useEffect(() => {
    if (selectedGroup) {
      loadGroupUsers();
    } else {
      setGroupUsers([]);
      setSelectedUserId('');
    }
  }, [selectedGroup]);

  const loadWorkOrder = async () => {
    if (!id) {
      navigate('/work-orders');
      return;
    }
    
    try {
      const data = await workOrderService.getById(id);
      setWorkOrder(data);
      setSelectedGroup(data.assignedGroup || '');
      setSelectedUserId(data.assignedToUserId || '');
    } catch (error: any) {
      console.error('Failed to load work order:', error);
      alert(`Failed to load work order: ${error.response?.data?.message || error.message}`);
      navigate('/work-orders');
    } finally {
      setLoading(false);
    }
  };

  const loadNotes = async () => {
    try {
      const data = await workOrderNoteService.getNotes(id!);
      setNotes(data);
    } catch (error) {
      console.error('Failed to load notes:', error);
    }
  };

  const loadTeams = async () => {
    try {
      const data = await userService.getAllTeams();
      setTeams(data);
    } catch (error) {
      console.error('Error loading teams:', error);
    }
  };

  const loadGroupUsers = async () => {
    try {
      const data = await userService.getByTeam(selectedGroup);
      setGroupUsers(data);
    } catch (error) {
      console.error('Error loading users:', error);
      setGroupUsers([]);
    }
  };

  const handleAddNote = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newNote.trim() || !id) return;

    setSubmittingNote(true);
    try {
      const note = await workOrderNoteService.addNote(id, { commentText: newNote });
      setNotes([...notes, note]);
      setNewNote('');
    } catch (error) {
      console.error('Failed to add note:', error);
      alert('Failed to add note');
    } finally {
      setSubmittingNote(false);
    }
  };

  const handleAssign = async () => {
    if (!id) return;

    setAssigning(true);
    try {
      const updated = await workOrderService.assign(id, {
        assignedGroup: selectedGroup || undefined,
        userId: selectedUserId || undefined,
      });
      setWorkOrder(updated);
      alert('Work order assigned successfully');
    } catch (error) {
      console.error('Failed to assign work order:', error);
      alert('Failed to assign work order');
    } finally {
      setAssigning(false);
    }
  };

  const handleStatusChange = async (newStatus: WorkOrderStatus) => {
    if (!id || !workOrder) return;

    setUpdatingStatus(true);
    try {
      const updated = await workOrderService.update(id, { status: newStatus });
      setWorkOrder(updated);
      alert('Status updated successfully');
    } catch (error) {
      console.error('Failed to update status:', error);
      alert('Failed to update status');
    } finally {
      setUpdatingStatus(false);
    }
  };

  const getPriorityColor = (priority: string) => {
    switch (priority.toLowerCase()) {
      case 'critical': return 'bg-red-100 text-red-800';
      case 'high': return 'bg-orange-100 text-orange-800';
      case 'medium': return 'bg-yellow-100 text-yellow-800';
      case 'low': return 'bg-green-100 text-green-800';
      default: return 'bg-gray-100 text-gray-800';
    }
  };

  const getStatusColor = (status: string) => {
    switch (status.toLowerCase()) {
      case 'draft': return 'bg-gray-100 text-gray-800';
      case 'scheduled': return 'bg-blue-100 text-blue-800';
      case 'assigned': return 'bg-cyan-100 text-cyan-800';
      case 'inprogress': return 'bg-purple-100 text-purple-800';
      case 'onhold': return 'bg-yellow-100 text-yellow-800';
      case 'completed': return 'bg-green-100 text-green-800';
      case 'verified': return 'bg-emerald-100 text-emerald-800';
      case 'cancelled': return 'bg-red-100 text-red-800';
      default: return 'bg-gray-100 text-gray-800';
    }
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center min-h-screen">
        <div className="text-center">
          <div className="text-xl font-semibold text-gray-900">Loading work order...</div>
        </div>
      </div>
    );
  }

  if (!workOrder) {
    return (
      <div className="flex justify-center items-center min-h-screen">
        <div className="text-center">
          <div className="text-xl font-semibold text-gray-900">Work order not found</div>
          <button
            onClick={() => navigate('/work-orders')}
            className="mt-4 px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
          >
            Back to Work Orders
          </button>
        </div>
      </div>
    );
  }

  return (
    <div>
      <button
        onClick={() => navigate('/work-orders')}
        className="text-blue-600 hover:text-blue-800 mb-4"
      >
        ← Back to Work Orders
      </button>

      <div className="flex items-start justify-between mb-6">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">{workOrder.workOrderNumber}</h1>
          <p className="mt-1 text-lg text-gray-600">{workOrder.title}</p>
        </div>
        <div className="flex gap-2">
          <span className={`inline-flex items-center px-3 py-1 rounded-full text-sm font-medium ${getPriorityColor(workOrder.priority)}`}>
            {workOrder.priority}
          </span>
          <span className={`inline-flex items-center px-3 py-1 rounded-full text-sm font-medium ${getStatusColor(workOrder.status)}`}>
            {workOrder.status.replace('InProgress', 'In Progress').replace('OnHold', 'On Hold')}
          </span>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-12 gap-6">
        {/* Main Content */}
        <div className="lg:col-span-8 space-y-6">
          {/* Description */}
          <div className="bg-white shadow rounded-lg p-6">
            <h2 className="text-lg font-medium text-gray-900 mb-4">Description</h2>
            <p className="text-gray-700 whitespace-pre-wrap">{workOrder.description}</p>
          </div>

          {/* Parts Required */}
          {workOrder.partsRequired && (
            <div className="bg-white shadow rounded-lg p-6">
              <h2 className="text-lg font-medium text-gray-900 mb-4">Parts Required</h2>
              <p className="text-gray-700 whitespace-pre-wrap">{workOrder.partsRequired}</p>
            </div>
          )}

          {/* Completion Notes */}
          {workOrder.completionNotes && (
            <div className="bg-white shadow rounded-lg p-6">
              <h2 className="text-lg font-medium text-gray-900 mb-4">Completion Notes</h2>
              <p className="text-gray-700 whitespace-pre-wrap">{workOrder.completionNotes}</p>
            </div>
          )}
        </div>

        {/* Sidebar */}
        <div className="lg:col-span-4 space-y-6">
          {/* Status Change */}
          <div className="bg-white shadow rounded-lg p-6">
            <h3 className="text-sm font-medium text-gray-500 mb-4">Change Status</h3>
            <select
              value={workOrder.status}
              onChange={(e) => handleStatusChange(e.target.value as WorkOrderStatus)}
              disabled={updatingStatus}
              className="w-full border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500 text-sm disabled:bg-gray-100 disabled:cursor-not-allowed"
            >
              {Object.values(WorkOrderStatus).map((status) => (
                <option key={status} value={status}>
                  {status.replace('InProgress', 'In Progress').replace('OnHold', 'On Hold')}
                </option>
              ))}
            </select>
            {updatingStatus && (
              <p className="mt-2 text-xs text-gray-500">Updating status...</p>
            )}
          </div>

          {/* Assignment Panel */}
          <div className="bg-white shadow rounded-lg p-6">
            <h3 className="text-sm font-medium text-gray-500 mb-4">Assignment</h3>
            <div className="space-y-4">
              <div>
                <label className="block text-xs font-medium text-gray-700 mb-1">
                  Assigned Group
                </label>
                <select
                  value={selectedGroup}
                  onChange={(e) => setSelectedGroup(e.target.value)}
                  className="w-full border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500 text-sm"
                >
                  <option value="">Select group...</option>
                  {teams.map((team) => (
                    <option key={team} value={team}>
                      {team}
                    </option>
                  ))}
                </select>
              </div>

              {selectedGroup && (
                <div>
                  <label className="block text-xs font-medium text-gray-700 mb-1">
                    Assigned To
                  </label>
                  <select
                    value={selectedUserId}
                    onChange={(e) => setSelectedUserId(e.target.value)}
                    className="w-full border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500 text-sm"
                    disabled={!groupUsers.length}
                  >
                    <option value="">Select user...</option>
                    {groupUsers.map((user) => (
                      <option key={user.id} value={user.id}>
                        {user.fullName}
                      </option>
                    ))}
                  </select>
                </div>
              )}

              <button
                onClick={handleAssign}
                disabled={assigning || (!selectedGroup && !selectedUserId)}
                className="w-full px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 disabled:bg-gray-400 disabled:cursor-not-allowed text-sm"
              >
                {assigning ? 'Assigning...' : 'Assign Work Order'}
              </button>
            </div>
          </div>

          {/* Details */}
          <div className="bg-white shadow rounded-lg p-6">
            <h3 className="text-sm font-medium text-gray-500 mb-4">Details</h3>
            <dl className="space-y-3">
              <div>
                <dt className="text-xs font-medium text-gray-500">Type</dt>
                <dd className="mt-1 text-sm text-gray-900">{workOrder.type}</dd>
              </div>

              {workOrder.serviceName && (
                <div>
                  <dt className="text-xs font-medium text-gray-500">Service</dt>
                  <dd className="mt-1 text-sm text-gray-900">{workOrder.serviceName}</dd>
                </div>
              )}

              {workOrder.incidentNumber && (
                <div>
                  <dt className="text-xs font-medium text-gray-500">Related Incident</dt>
                  <dd className="mt-1 text-sm text-gray-900">
                    <a href={`/incidents/${workOrder.incidentId}`} className="text-blue-600 hover:text-blue-800">
                      {workOrder.incidentNumber}
                    </a>
                  </dd>
                </div>
              )}

              {workOrder.location && (
                <div>
                  <dt className="text-xs font-medium text-gray-500">Location</dt>
                  <dd className="mt-1 text-sm text-gray-900">{workOrder.location}</dd>
                </div>
              )}

              <div>
                <dt className="text-xs font-medium text-gray-500">Assigned To</dt>
                <dd className="mt-1 text-sm text-gray-900">
                  {workOrder.assignedToUserName || workOrder.assignedGroup || 'Unassigned'}
                </dd>
              </div>

              <div>
                <dt className="text-xs font-medium text-gray-500">Created By</dt>
                <dd className="mt-1 text-sm text-gray-900">{workOrder.createdByUserName}</dd>
              </div>

              <div>
                <dt className="text-xs font-medium text-gray-500">Created At</dt>
                <dd className="mt-1 text-sm text-gray-900">
                  {new Date(workOrder.createdAt).toLocaleString()}
                </dd>
              </div>

              {workOrder.completedAt && (
                <div>
                  <dt className="text-xs font-medium text-gray-500">Completed At</dt>
                  <dd className="mt-1 text-sm text-gray-900">
                    {new Date(workOrder.completedAt).toLocaleString()}
                  </dd>
                </div>
              )}
            </dl>
          </div>

          {/* Schedule */}
          <div className="bg-white shadow rounded-lg p-6">
            <h3 className="text-sm font-medium text-gray-500 mb-4">Schedule</h3>
            <dl className="space-y-3">
              {workOrder.scheduledStartDate && (
                <div>
                  <dt className="text-xs font-medium text-gray-500">Scheduled Start</dt>
                  <dd className="mt-1 text-sm text-gray-900">
                    {new Date(workOrder.scheduledStartDate).toLocaleString()}
                  </dd>
                </div>
              )}

              {workOrder.scheduledEndDate && (
                <div>
                  <dt className="text-xs font-medium text-gray-500">Scheduled End</dt>
                  <dd className="mt-1 text-sm text-gray-900">
                    {new Date(workOrder.scheduledEndDate).toLocaleString()}
                  </dd>
                </div>
              )}

              {workOrder.actualStartDate && (
                <div>
                  <dt className="text-xs font-medium text-gray-500">Actual Start</dt>
                  <dd className="mt-1 text-sm text-gray-900">
                    {new Date(workOrder.actualStartDate).toLocaleString()}
                  </dd>
                </div>
              )}

              {workOrder.actualEndDate && (
                <div>
                  <dt className="text-xs font-medium text-gray-500">Actual End</dt>
                  <dd className="mt-1 text-sm text-gray-900">
                    {new Date(workOrder.actualEndDate).toLocaleString()}
                  </dd>
                </div>
              )}
            </dl>
          </div>

          {/* Cost & Hours */}
          <div className="bg-white shadow rounded-lg p-6">
            <h3 className="text-sm font-medium text-gray-500 mb-4">Cost & Hours</h3>
            <dl className="space-y-3">
              {workOrder.estimatedCost != null && (
                <div>
                  <dt className="text-xs font-medium text-gray-500">Estimated Cost</dt>
                  <dd className="mt-1 text-sm text-gray-900">${workOrder.estimatedCost.toFixed(2)}</dd>
                </div>
              )}

              {workOrder.actualCost != null && (
                <div>
                  <dt className="text-xs font-medium text-gray-500">Actual Cost</dt>
                  <dd className="mt-1 text-sm text-gray-900">${workOrder.actualCost.toFixed(2)}</dd>
                </div>
              )}

              {workOrder.estimatedHours != null && (
                <div>
                  <dt className="text-xs font-medium text-gray-500">Estimated Hours</dt>
                  <dd className="mt-1 text-sm text-gray-900">{workOrder.estimatedHours}h</dd>
                </div>
              )}

              {workOrder.actualHours != null && (
                <div>
                  <dt className="text-xs font-medium text-gray-500">Actual Hours</dt>
                  <dd className="mt-1 text-sm text-gray-900">{workOrder.actualHours}h</dd>
                </div>
              )}
            </dl>
          </div>
        </div>
      </div>

      {/* Notes Section */}
      <div className="mt-8 bg-white shadow rounded-lg p-6">
        <h2 className="text-lg font-medium text-gray-900 mb-4">Notes</h2>
        
        {/* Add Note Form */}
        <form onSubmit={handleAddNote} className="mb-6">
          <textarea
            value={newNote}
            onChange={(e) => setNewNote(e.target.value)}
            placeholder="Add a note..."
            rows={3}
            className="w-full border border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500 p-3"
            disabled={submittingNote}
          />
          <div className="mt-2 flex justify-end">
            <button
              type="submit"
              disabled={!newNote.trim() || submittingNote}
              className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 disabled:bg-gray-400 disabled:cursor-not-allowed"
            >
              {submittingNote ? 'Adding...' : 'Add Note'}
            </button>
          </div>
        </form>

        {/* Notes List */}
        <div className="space-y-4">
          {notes.length === 0 ? (
            <p className="text-gray-500 text-sm">No notes yet</p>
          ) : (
            notes.map((note) => (
              <div key={note.id} className="border-l-4 border-blue-500 pl-4 py-2">
                <div className="flex items-center justify-between mb-2">
                  <span className="font-medium text-gray-900">{note.userName}</span>
                  <span className="text-sm text-gray-500">
                    {new Date(note.createdAt).toLocaleString()}
                  </span>
                </div>
                <p className="text-gray-700 whitespace-pre-wrap">{note.commentText}</p>
              </div>
            ))
          )}
        </div>
      </div>
    </div>
  );
};

export default WorkOrderDetailsPage;
