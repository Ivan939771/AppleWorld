# Deploying AppleWorld EMS to Render (render.com)

This guide provides step-by-step instructions to deploy the **AppleWorld Shopping Mall Employee Management System (.NET 8 MVC)** onto **Render Server**.

---

## 🚀 Quick Start: Deploying with Render Blueprint (Recommended)

1. **Push your code to GitHub / GitLab**
   Ensure your repository includes:
   - `Dockerfile`
   - `.dockerignore`
   - `render.yaml`
   - The `APPLEWORLD/` project folder

2. **Connect to Render**
   - Log into your [Render Dashboard](https://dashboard.render.com/).
   - Click **New +** in the top right and select **Blueprint**.
   - Connect your GitHub / GitLab account and select your `AppleWorld` repository.

3. **Deploy**
   - Render will automatically read `render.yaml` and configure a Docker Web Service named `appleworld-ems`.
   - Click **Apply** to start the build and deployment process.
   - Once deployed, Render will provide a public live URL (e.g. `https://appleworld-ems.onrender.com`).

---

## 🛠️ Alternative: Manual Web Service Setup on Render

If you prefer to configure the Web Service manually without using `render.yaml`:

1. Go to [Render Dashboard](https://dashboard.render.com/) -> **New +** -> **Web Service**.
2. Select **Build and deploy from a Git repository**.
3. Select your repository.
4. Fill in the service configuration:
   - **Name**: `appleworld-ems`
   - **Language / Runtime**: `Docker`
   - **Dockerfile Path**: `./Dockerfile`
   - **Docker Context**: `.`
   - **Instance Type**: `Free` (or desired tier)
5. Add Environment Variables:
   - `ASPNETCORE_ENVIRONMENT` = `Production`
   - `ASPNETCORE_URLS` = `http://+:10000`
6. Click **Create Web Service**.

---

## 🔑 Default Administrator Logins

When the application launches for the first time, EF Core will automatically create and seed the database with the default admin:

- **Login URL**: `https://<your-render-app>.onrender.com/Account/Login`
- **Username**: `admin`
- **Password**: `Admin@123`

> ⚠️ **Important Security Note**: Be sure to log in and change the administrator password or add new admin users before going live in production.

---

## 💾 SQLite Database Persistence (Optional Render Disk)

By default, SQLite creates `appleworld.db` inside the container directory. On Render's Free tier, container files reset whenever a new deployment builds.

To preserve database changes across deployments on Render:
1. Attach a **Render Persistent Disk** (e.g., mounted at `/app/data`).
2. Add the following Environment Variable in your Render Web Service settings:
   - **Key**: `ConnectionStrings__DefaultConnection`
   - **Value**: `Data Source=/app/data/appleworld.db`

---

## 🔍 Troubleshooting & Logs

- **Health Check**: Render pings port `10000` to confirm the container is healthy.
- **Viewing Logs**: Access live application runtime logs directly under the **Logs** tab in your Render dashboard.
