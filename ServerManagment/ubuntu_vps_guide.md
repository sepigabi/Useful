# First Steps – Ubuntu Server (Forpsi VPS) – Detailed Guide

This guide is for users who are new to Linux (Ubuntu Server) VPS hosting and are coming from a Windows background.
It covers connecting to your server, initial configurations, file transfers, software installation, firewall setup, process/service management, SSH key authentication, DNS basics, and useful commands.

---

## 1. Logging in from a Windows Laptop (SSH)

### What you need
- VPS **IP address** (found in your Forpsi admin panel)
- Username (initially `root`)
- Password (provided by Forpsi when the VPS was created)

### Login from PowerShell
```powershell
ssh root@123.45.67.89
```
On first login, you’ll see:
```
The authenticity of host '123.45.67.89' can't be established.
Are you sure you want to continue connecting (yes/no/[fingerprint])?
```
Type: `yes`

Then enter your password. If correct, you’ll see something like:
```
Welcome to Ubuntu 22.04 LTS (GNU/Linux 5.15.0-76-generic x86_64)

System information as of Mon Aug 11 20:14:59 UTC 2025

  System load:  0.01              Memory usage:  12%
  Usage of /:   3% of 40GB        Swap usage:    0%
  IP address:   123.45.67.89
  => There are 5 updates available for your system.

Last login: Mon Aug 11 20:14:59 2025 from 77.88.99.111
root@vps:~#
```
The `root@vps:~#` prompt indicates you’re logged in as root.

**Difference from Windows:** no graphical interface, everything is done in the terminal.

---

## 2. SSH Key Authentication (Passwordless Login)

SSH keys are more secure than passwords and prevent brute-force attacks.

### How it works
- You generate a **key pair**: a private key (kept on your computer) and a public key (stored on the server).
- When connecting, the server checks if your private key matches the stored public key.

### Setup on Windows (PowerShell)
1. Generate keys:
```powershell
ssh-keygen -t rsa -b 4096 -C "you@example.com"
```
Press Enter to accept the default save path (`C:\Users\YourUser\.ssh\id_rsa`) and set a passphrase (optional but recommended).

2. Copy the public key to the server:
```powershell
scp C:\Users\YourUser\.ssh\id_rsa.pub user@123.45.67.89:~
```

3. On the server:
```bash
mkdir -p ~/.ssh
cat ~/id_rsa.pub >> ~/.ssh/authorized_keys
chmod 600 ~/.ssh/authorized_keys
chmod 700 ~/.ssh
rm ~/id_rsa.pub
```

4. Disable password authentication for extra security:
```bash
sudo nano /etc/ssh/sshd_config
# Change or add:
PasswordAuthentication no
PermitRootLogin no
sudo systemctl restart ssh
```

**Note:** SSH keys can be used from any machine that has the **private key**.

---

## 3. File Transfers

### Upload a single file
```powershell
scp C:\projects\index.html user@123.45.67.89:/var/www/html/
```

### Upload a whole folder
```powershell
scp -r C:\projects\app user@123.45.67.89:/var/www/
```

### Download from server
```powershell
scp user@123.45.67.89:/var/www/html/index.html C:\downloads\
```

### GUI method – WinSCP
- Download from: https://winscp.net
- Login with IP, username, password/SSH key for a two-panel file manager.

---

## 4. Creating a New User and Disabling Root Login

1. Create a new user:
```bash
sudo adduser newuser
sudo usermod -aG sudo newuser
```
The password you set here will be the login password for this user.

2. Disable root login in SSH config:
```bash
sudo nano /etc/ssh/sshd_config
# Change:
PermitRootLogin no
sudo systemctl restart ssh
```

---

## 5. Initial System Configuration

### Update system
```bash
sudo apt update && sudo apt upgrade -y
```

### Set timezone
```bash
sudo timedatectl set-timezone Europe/Budapest
```

### Install useful tools
```bash
sudo apt install htop net-tools curl unzip git ufw -y
```
- **htop** → like Windows Task Manager in terminal
- **net-tools** → `netstat`, `ifconfig` etc.
- **curl** → test HTTP requests
- **unzip** → extract ZIP files
- **git** → version control
- **ufw** → firewall

---

## 6. Firewall (UFW)

Enable UFW and allow SSH:
```bash
sudo ufw allow OpenSSH
sudo ufw enable
```

Open/close ports:
```bash
sudo ufw allow 80/tcp   # HTTP
sudo ufw allow 443/tcp  # HTTPS
sudo ufw deny 3306/tcp  # Block MySQL from outside
```

List firewall rules:
```bash
sudo ufw status verbose
```
Example output:
```
Status: active
To                         Action      From
--                         ------      ----
22/tcp                     ALLOW       Anywhere
80/tcp                     ALLOW       Anywhere
443/tcp                    ALLOW       Anywhere
3306/tcp                   DENY        Anywhere
```

---

## 7. Managing Software and Services

### List installed packages
```bash
dpkg --get-selections
```

### Remove a package
```bash
sudo apt remove packagename
```

### Remove a package and config files
```bash
sudo apt purge packagename
```

### View running processes (like Task Manager)
```bash
htop
```

### List running services
```bash
systemctl list-units --type=service --state=running
```

---

## 8. Basic Linux Commands (Windows equivalents)

| Windows   | Linux    | Description |
|-----------|----------|-------------|
| `dir`     | `ls`     | List directory contents |
| `cd`      | `cd`     | Change directory |
| `copy`    | `cp`     | Copy file |
| `del`     | `rm`     | Delete file |
| `mkdir`   | `mkdir`  | Create folder |
| `tasklist`| `ps aux` | List processes |
| `taskkill /PID` | `kill <PID>` | Kill process |

---

## 9. DNS Basics for Domains & Subdomains

- **A record** → maps a domain to an IPv4 address.
- **CNAME record** → alias to another domain.
- Example:  
  `api.example.com` (A record) → VPS IP address.

---

## 10. Example Web App Folder Structure

```
/var/www/
    landing/       # main site
    app1/          # ASP.NET Core app
    app2/          # Angular app
```

---

## 11. Useful Monitoring Commands

- Disk usage:  
  ```bash
  df -h
  ```
- Memory usage:  
  ```bash
  free -m
  ```
- Network ports:  
  ```bash
  sudo netstat -tulnp
  ```

---

## 12. Final Recommendations

- Use strong passwords or SSH keys.
- Update regularly:  
  ```bash
  sudo apt update && sudo apt upgrade -y
  ```
- Keep unused ports closed.
- Make regular backups (`tar`, `rsync`, WinSCP).
