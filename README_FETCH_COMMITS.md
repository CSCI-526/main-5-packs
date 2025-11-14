# Fetch October 2025 Commit Links

This directory contains a script to fetch commit links from the last week of October 2025.

## Overview

The `fetch_october_commits.sh` script queries the Git repository for commits made during the last week of October 2025 (October 24-31, 2025) and generates GitHub commit links for easy access.

## Usage

### Basic Usage

To fetch all commits from the last week of October 2025:

```bash
bash fetch_october_commits.sh
```

or simply:

```bash
./fetch_october_commits.sh
```

### Filter by Author

To fetch only your commits (or commits by a specific author):

```bash
bash fetch_october_commits.sh --author "Your Name"
```

You can also filter by email:

```bash
bash fetch_october_commits.sh --author "your.email@example.com"
```

### Output Formats

The script supports three output formats:

#### Default Format (Human-Readable)

```bash
bash fetch_october_commits.sh
```

Output:
```
=====================================================================
Commits from Last Week of October 2025 (October 24-31, 2025)
Total commits found: 5
=====================================================================

Commit: Fixed bug in level 3
Author: John Doe <john.doe@example.com>
Date: 2025-10-28 14:23:45 -0700
Link: https://github.com/CSCI-526/main-5-packs/commit/abc123...
---------------------------------------------------------------------
...
```

#### Markdown Format

```bash
bash fetch_october_commits.sh --format markdown
```

Output:
```
- [Fixed bug in level 3](https://github.com/CSCI-526/main-5-packs/commit/abc123...) - John Doe (2025-10-28 14:23:45 -0700)
- [Added new feature](https://github.com/CSCI-526/main-5-packs/commit/def456...) - Jane Smith (2025-10-27 10:15:30 -0700)
```

#### JSON Format

```bash
bash fetch_october_commits.sh --format json
```

Output:
```json
[
  {
    "sha": "abc123...",
    "author": "John Doe",
    "email": "john.doe@example.com",
    "date": "2025-10-28 14:23:45 -0700",
    "message": "Fixed bug in level 3",
    "url": "https://github.com/CSCI-526/main-5-packs/commit/abc123..."
  },
  ...
]
```

### Combining Options

You can combine the author filter with output formats:

```bash
bash fetch_october_commits.sh --author "John Doe" --format markdown
```

## Examples

### Example 1: Get Your Commits
```bash
bash fetch_october_commits.sh --author "kvenugop@usc.edu"
```

### Example 2: Generate a Markdown Report
```bash
bash fetch_october_commits.sh --format markdown > october_commits.md
```

### Example 3: Get JSON Data for Processing
```bash
bash fetch_october_commits.sh --format json > october_commits.json
```

### Example 4: Filter by Author Name
```bash
bash fetch_october_commits.sh --author "Karthik"
```

## Requirements

- Git must be installed and available in the PATH
- The script must be run from within the Git repository (or have the repository accessible)
- Bash shell (version 3.0 or higher)

## Date Range

The script specifically fetches commits from:
- **Start Date**: October 24, 2025 (00:00:00)
- **End Date**: October 31, 2025 (23:59:59)

This represents the last full week of October 2025.

## Notes

- If no commits are found in the specified date range, the script will display a message indicating this.
- The script searches all branches using `git log --all`.
- Author filtering is case-sensitive and matches both author name and email.

## Troubleshooting

### No commits found
If you see "No commits found in the last week of October 2025", this means:
- There were no commits made during that period, OR
- The author filter didn't match any commits

### Permission denied
If you get a "Permission denied" error, make the script executable:
```bash
chmod +x fetch_october_commits.sh
```

Or run it with bash explicitly:
```bash
bash fetch_october_commits.sh
```

## Help

For detailed help and options:
```bash
bash fetch_october_commits.sh --help
```
