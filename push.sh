#!/bin/bash

eval `ssh-agent -s`
ssh-add "/c/Users/SouthPAW Games/.ssh/id_rsa"
git push
