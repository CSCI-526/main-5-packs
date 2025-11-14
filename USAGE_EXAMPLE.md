# Quick Start: Fetching October 2025 Commit Links

## How to get YOUR commit links from last week of October 2025

### Step 1: Navigate to the repository
```bash
cd /path/to/main-5-packs
```

### Step 2: Run the script with your author name or email

**Option A: Filter by your name**
```bash
bash fetch_october_commits.sh --author "Your Name"
```

**Option B: Filter by your email**
```bash
bash fetch_october_commits.sh --author "your.email@usc.edu"
```

### Step 3: Choose your preferred output format

**Human-readable format (default):**
```bash
bash fetch_october_commits.sh --author "your.email@usc.edu"
```

**Markdown format (great for reports):**
```bash
bash fetch_october_commits.sh --author "your.email@usc.edu" --format markdown
```

**JSON format (for data processing):**
```bash
bash fetch_october_commits.sh --author "your.email@usc.edu" --format json
```

## Common Examples

### Example 1: Get Karthik's commits
```bash
bash fetch_october_commits.sh --author "kvenugop@usc.edu"
```

### Example 2: Get all commits in markdown format
```bash
bash fetch_october_commits.sh --format markdown
```

### Example 3: Save your commits to a file
```bash
bash fetch_october_commits.sh --author "your.email@usc.edu" > my_october_commits.txt
```

### Example 4: Create a markdown report of your commits
```bash
bash fetch_october_commits.sh --author "your.email@usc.edu" --format markdown > my_commits.md
```

## Current Status

As of November 14, 2025, there are **no commits** in the repository from the last week of October 2025 (October 24-31, 2025). 

When you run the script, you will see:
```
No commits found in the last week of October 2025 (Oct 24-31)
```

This is expected and accurate based on the repository's current commit history.

## What the script does

The `fetch_october_commits.sh` script:
1. Searches all branches for commits between October 24-31, 2025
2. Optionally filters by author (name or email)
3. Generates GitHub commit links for each commit found
4. Outputs in your chosen format (default, markdown, or JSON)

## For more help

View the full documentation:
```bash
cat README_FETCH_COMMITS.md
```

View script help:
```bash
bash fetch_october_commits.sh --help
```
