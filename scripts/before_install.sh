#!/bin/bash
cat <<EOF > /etc/systemd/system/libraryapi.service
[Unit]
Description=LibraryAPI Service
After=network.target

[Service]
WorkingDirectory=/var/www/LibraryAPI
ExecStart=/usr/bin/dotnet run
Restart=always
RestartSec=10
Environment=DOTNET_CLI_HOME=/tmp
Environment=ASPNETCORE_URLS=http://*:80
SyslogIdentifier=libraryapi

[Install]
WantedBy=multi-user.target
EOF

systemctl daemon-reload
systemctl enable libraryapi.service