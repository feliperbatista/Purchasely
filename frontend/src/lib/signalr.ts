import * as signalR from '@microsoft/signalr';

let connection: signalR.HubConnection | null = null;

export function getConnection() {
  if (!connection)
    connection = new signalR.HubConnectionBuilder()
      .withUrl('/hubs/notifications')
      .withAutomaticReconnect()
      .build();
  return connection;
}

export async function startConnection() {
  const conn = getConnection();
  if (conn.state === signalR.HubConnectionState.Disconnected)
    await conn.start();
  return conn;
}

export async function stopConnection() {
  const conn = getConnection();
  if (conn.state === signalR.HubConnectionState.Connected)
    await conn.stop();
  return conn;
}
