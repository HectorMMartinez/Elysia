import UserStatusBadge from "./UserStatusBadge";
import UserActions from "./UserActions";

export default function UserTable({
  users,
  loading,
  onView,
  onActivate,
  onDeactivate,
}) {
  if (loading) {
    return (
      <div className="bg-white rounded-2xl shadow-sm border border-slate-100 p-10">
        <div className="flex justify-center items-center">
          <div className="w-8 h-8 border-4 border-violet-200 border-t-violet-600 rounded-full animate-spin" />
        </div>

        <p className="text-center text-slate-500 mt-4">
          Cargando usuarios...
        </p>
      </div>
    );
  }

  if (!users || users.length === 0) {
    return (
      <div className="bg-white rounded-2xl shadow-sm border border-slate-100 p-10 text-center">
        <p className="text-slate-500">
          No existen usuarios registrados.
        </p>
      </div>
    );
  }

  return (
    <div className="bg-white rounded-2xl shadow-sm border border-slate-100 overflow-hidden">
      <div className="overflow-x-auto">
        <table className="w-full">
          <thead>
            <tr className="bg-slate-50 border-b border-slate-100">
              <th className="text-left px-6 py-4 text-xs font-semibold text-slate-500 uppercase">
                Usuario
              </th>

              <th className="text-left px-6 py-4 text-xs font-semibold text-slate-500 uppercase">
                Username
              </th>

              <th className="text-left px-6 py-4 text-xs font-semibold text-slate-500 uppercase">
                Email
              </th>

              <th className="text-left px-6 py-4 text-xs font-semibold text-slate-500 uppercase">
                Teléfono
              </th>

              <th className="text-left px-6 py-4 text-xs font-semibold text-slate-500 uppercase">
                Estado
              </th>

              <th className="text-right px-6 py-4 text-xs font-semibold text-slate-500 uppercase">
                Acciones
              </th>
            </tr>
          </thead>

          <tbody className="divide-y divide-slate-100">
            {users.map((user) => (
              <tr
                key={user.id}
                className="hover:bg-slate-50 transition"
              >
                {/* Usuario */}
                <td className="px-6 py-4">
                  <div className="flex items-center gap-3">
                    <img
                      src={
                        user.profileImage ||
                        "https://cdn-icons-png.flaticon.com/512/149/149071.png"
                      }
                      alt={`${user.name} ${user.lastName}`}
                      className="w-10 h-10 rounded-full object-cover border border-slate-200"
                      onError={(e) => {
                        e.currentTarget.src =
                          "https://cdn-icons-png.flaticon.com/512/149/149071.png";
                      }}
                    />

                    <div>
                      <p className="font-semibold text-slate-800">
                        {user.name} {user.lastName}
                      </p>

                      <p className="text-xs text-slate-400">
                        {user.role}
                      </p>
                    </div>
                  </div>
                </td>

                {/* Username */}
                <td className="px-6 py-4">
                  <span className="text-sm text-slate-600">
                    @{user.userName}
                  </span>
                </td>

                {/* Email */}
                <td className="px-6 py-4">
                  <span className="text-sm text-slate-600">
                    {user.email}
                  </span>
                </td>

                {/* Teléfono */}
                <td className="px-6 py-4">
                  <span className="text-sm text-slate-600">
                    {user.phone || "No registrado"}
                  </span>
                </td>

                {/* Estado */}
                <td className="px-6 py-4">
                  <UserStatusBadge isActive={user.isActive} />
                </td>

                {/* Acciones */}
                <td className="px-6 py-4">
                  <UserActions
                    user={user}
                    onView={onView}
                    onActivate={onActivate}
                    onDeactivate={onDeactivate}
                  />
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}