import { useState, useEffect, FormEvent } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { incidentService } from '../services/incidentService';
import { commentService } from '../services/commentService';
import { Incident, IncidentComment, IncidentStatus } from '../types';
import { useAuth } from '../context/AuthContext';
import { signalRService } from '../services/signalRService';

const IncidentDetailsPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { user } = useAuth();
  const [incident, setIncident] = useState<Incident | null>(null);
  const [comments, setComments] = useState<IncidentComment[]>([]);
  const [newComment, setNewComment] = useState('');
  const [loading, setLoading] = useState(true);
  const [submittingComment, setSubmittingComment] = useState(false);

  useEffect(() => {
    if (id) {
      loadIncident();
      loadComments();

      // Setup SignalR listeners for real-time updates
      signalRService.on('IncidentUpdated', handleIncidentUpdate);
      signalRService.on('CommentAdded', handleCommentAdded);

      return () => {
        signalRService.off('IncidentUpdated', handleIncidentUpdate);
        signalRService.off('CommentAdded', handleCommentAdded);
      };
    }
  }, [id]);

  const handleIncidentUpdate = (updatedIncident: Incident) => {
    if (updatedIncident.id === id) {
      setIncident(updatedIncident);
    }
  };

  const handleCommentAdded = (comment: IncidentComment) => {
    if (comment.incidentId === id) {
      setComments((prev) => [...prev, comment]);
    }
  };

  const loadIncident = async () => {
    try {
      const data = await incidentService.getById(id!);
      setIncident(data);
    } catch (error) {
      console.error('Failed to load incident:', error);
    } finally {
      setLoading(false);
    }
  };

  const loadComments = async () => {
    try {
      const data = await commentService.getComments(id!);
      setComments(data);
    } catch (error) {
      console.error('Failed to load comments:', error);
    }
  };

  const handleStatusChange = async (newStatus: string) => {
    try {
      const updated = await incidentService.updateStatus(id!, newStatus);
      setIncident(updated);
    } catch (error) {
      console.error('Failed to update status:', error);
    }
  };

  const handleSubmitComment = async (e: FormEvent) => {
    e.preventDefault();
    if (!newComment.trim()) return;

    setSubmittingComment(true);
    try {
      await commentService.createComment(id!, { commentText: newComment });
      setNewComment('');
      // Comment will be added via SignalR
    } catch (error) {
      console.error('Failed to submit comment:', error);
    } finally {
      setSubmittingComment(false);
    }
  };

  if (loading) {
    return <div className="text-center py-10">Loading...</div>;
  }

  if (!incident) {
    return <div className="text-center py-10">Incident not found</div>;
  }

  return (
    <div className="px-4 py-6">
      <div className="mb-6">
        <button
          onClick={() => navigate('/incidents')}
          className="text-blue-600 hover:text-blue-800 mb-4"
        >
          ← Back to Incidents
        </button>
        <div className="flex justify-between items-start">
          <div>
            <h1 className="text-3xl font-bold text-gray-900">{incident.incidentNumber}</h1>
            <p className="mt-1 text-xl text-gray-700">{incident.title}</p>
          </div>
          <span className={`px-3 py-1 inline-flex text-sm leading-5 font-semibold rounded-full ${
            incident.severity === 'SEV1' ? 'bg-red-100 text-red-800' :
            incident.severity === 'SEV2' ? 'bg-orange-100 text-orange-800' :
            incident.severity === 'SEV3' ? 'bg-yellow-100 text-yellow-800' :
            'bg-green-100 text-green-800'
          }`}>
            {incident.severity}
          </span>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Main Content */}
        <div className="lg:col-span-2 space-y-6">
          {/* Incident Details */}
          <div className="bg-white shadow rounded-lg p-6">
            <h2 className="text-lg font-medium text-gray-900 mb-4">Description</h2>
            <p className="text-gray-700 whitespace-pre-wrap">{incident.description}</p>
          </div>

          {/* Comments Section */}
          <div className="bg-white shadow rounded-lg p-6">
            <h2 className="text-lg font-medium text-gray-900 mb-4">
              Comments ({comments.length})
            </h2>

            {/* Comment Form */}
            <form onSubmit={handleSubmitComment} className="mb-6">
              <textarea
                className="w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                rows={3}
                placeholder="Add a comment..."
                value={newComment}
                onChange={(e) => setNewComment(e.target.value)}
              />
              <div className="mt-2 flex justify-end">
                <button
                  type="submit"
                  disabled={submittingComment || !newComment.trim()}
                  className="px-4 py-2 border border-transparent text-sm font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700 disabled:opacity-50"
                >
                  {submittingComment ? 'Posting...' : 'Post Comment'}
                </button>
              </div>
            </form>

            {/* Comments List */}
            <div className="space-y-4">
              {comments.map((comment) => (
                <div key={comment.id} className="border-l-4 border-blue-500 pl-4 py-2">
                  <div className="flex justify-between items-start mb-1">
                    <span className="text-sm font-semibold text-gray-900">
                      {comment.userFullName}
                    </span>
                    <span className="text-xs text-gray-500">
                      {new Date(comment.createdAt).toLocaleString()}
                    </span>
                  </div>
                  <p className="text-gray-700">{comment.commentText}</p>
                </div>
              ))}
              {comments.length === 0 && (
                <p className="text-gray-500 text-center py-4">No comments yet</p>
              )}
            </div>
          </div>
        </div>

        {/* Sidebar */}
        <div className="space-y-6">
          {/* Status */}
          <div className="bg-white shadow rounded-lg p-6">
            <h3 className="text-sm font-medium text-gray-500 mb-2">Status</h3>
            <select
              className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
              value={incident.status}
              onChange={(e) => handleStatusChange(e.target.value)}
            >
              <option value={IncidentStatus.Open}>Open</option>
              <option value={IncidentStatus.Assigned}>Assigned</option>
              <option value={IncidentStatus.InProgress}>In Progress</option>
              <option value={IncidentStatus.Resolved}>Resolved</option>
              <option value={IncidentStatus.Closed}>Closed</option>
            </select>
          </div>

          {/* Details */}
          <div className="bg-white shadow rounded-lg p-6">
            <h3 className="text-sm font-medium text-gray-500 mb-4">Details</h3>
            <dl className="space-y-3">
              <div>
                <dt className="text-xs font-medium text-gray-500">Service</dt>
                <dd className="mt-1 text-sm text-gray-900">{incident.serviceName}</dd>
              </div>
              <div>
                <dt className="text-xs font-medium text-gray-500">Assigned To</dt>
                <dd className="mt-1 text-sm text-gray-900">
                  {incident.assignedToUserName || 'Unassigned'}
                </dd>
              </div>
              <div>
                <dt className="text-xs font-medium text-gray-500">Created By</dt>
                <dd className="mt-1 text-sm text-gray-900">{incident.createdByUserName}</dd>
              </div>
              <div>
                <dt className="text-xs font-medium text-gray-500">Created At</dt>
                <dd className="mt-1 text-sm text-gray-900">
                  {new Date(incident.createdAt).toLocaleString()}
                </dd>
              </div>
              {incident.resolvedAt && (
                <div>
                  <dt className="text-xs font-medium text-gray-500">Resolved At</dt>
                  <dd className="mt-1 text-sm text-gray-900">
                    {new Date(incident.resolvedAt).toLocaleString()}
                  </dd>
                </div>
              )}
            </dl>
          </div>

          {/* SLA Info */}
          <div className="bg-white shadow rounded-lg p-6">
            <h3 className="text-sm font-medium text-gray-500 mb-4">SLA</h3>
            <dl className="space-y-3">
              {incident.responseDueAt && (
                <div>
                  <dt className="text-xs font-medium text-gray-500">Response Due</dt>
                  <dd className={`mt-1 text-sm ${incident.isResponseBreached ? 'text-red-600 font-semibold' : 'text-gray-900'}`}>
                    {new Date(incident.responseDueAt).toLocaleString()}
                    {incident.isResponseBreached && ' (BREACHED)'}
                  </dd>
                </div>
              )}
              {incident.resolutionDueAt && (
                <div>
                  <dt className="text-xs font-medium text-gray-500">Resolution Due</dt>
                  <dd className={`mt-1 text-sm ${incident.isResolutionBreached ? 'text-red-600 font-semibold' : 'text-gray-900'}`}>
                    {new Date(incident.resolutionDueAt).toLocaleString()}
                    {incident.isResolutionBreached && ' (BREACHED)'}
                  </dd>
                </div>
              )}
            </dl>
          </div>
        </div>
      </div>
    </div>
  );
};

export default IncidentDetailsPage;
