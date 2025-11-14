# Demo: fetch_october_commits.sh Output Examples

## What the script does

The script fetches commits from the last week of October 2025 (October 24-31) and generates GitHub commit links.

## Current Output

Since there are no commits from October 24-31, 2025 in the repository, the script correctly reports:

```
No commits found in the last week of October 2025 (Oct 24-31)
```

## Example Output (if commits existed)

### Default Format

```
=====================================================================
Commits from Last Week of October 2025 (October 24-31, 2025)
Total commits found: 3
=====================================================================

Commit: Fixed bug in level 3
Author: John Doe <john.doe@usc.edu>
Date: 2025-10-28 14:23:45 -0700
Link: https://github.com/CSCI-526/main-5-packs/commit/abc123def456...
---------------------------------------------------------------------
Commit: Added new feature to level 2
Author: Jane Smith <jane.smith@usc.edu>
Date: 2025-10-27 10:15:30 -0700
Link: https://github.com/CSCI-526/main-5-packs/commit/def456abc789...
---------------------------------------------------------------------
Commit: Updated documentation
Author: Bob Johnson <bob.johnson@usc.edu>
Date: 2025-10-26 16:45:22 -0700
Link: https://github.com/CSCI-526/main-5-packs/commit/789abc456def...
---------------------------------------------------------------------
```

### Markdown Format

```
- [Fixed bug in level 3](https://github.com/CSCI-526/main-5-packs/commit/abc123def456...) - John Doe (2025-10-28 14:23:45 -0700)
- [Added new feature to level 2](https://github.com/CSCI-526/main-5-packs/commit/def456abc789...) - Jane Smith (2025-10-27 10:15:30 -0700)
- [Updated documentation](https://github.com/CSCI-526/main-5-packs/commit/789abc456def...) - Bob Johnson (2025-10-26 16:45:22 -0700)
```

### JSON Format

```json
[
  {
    "sha": "abc123def456...",
    "author": "John Doe",
    "email": "john.doe@usc.edu",
    "date": "2025-10-28 14:23:45 -0700",
    "message": "Fixed bug in level 3",
    "url": "https://github.com/CSCI-526/main-5-packs/commit/abc123def456..."
  },
  {
    "sha": "def456abc789...",
    "author": "Jane Smith",
    "email": "jane.smith@usc.edu",
    "date": "2025-10-27 10:15:30 -0700",
    "message": "Added new feature to level 2",
    "url": "https://github.com/CSCI-526/main-5-packs/commit/def456abc789..."
  },
  {
    "sha": "789abc456def...",
    "author": "Bob Johnson",
    "email": "bob.johnson@usc.edu",
    "date": "2025-10-26 16:45:22 -0700",
    "message": "Updated documentation",
    "url": "https://github.com/CSCI-526/main-5-packs/commit/789abc456def..."
  }
]
```

### With Author Filter

```bash
bash fetch_october_commits.sh --author "john.doe@usc.edu"
```

Output:
```
=====================================================================
Commits from Last Week of October 2025 (October 24-31, 2025)
Filtered by author: john.doe@usc.edu
Total commits found: 1
=====================================================================

Commit: Fixed bug in level 3
Author: John Doe <john.doe@usc.edu>
Date: 2025-10-28 14:23:45 -0700
Link: https://github.com/CSCI-526/main-5-packs/commit/abc123def456...
---------------------------------------------------------------------
```

## How to Use

1. **Get all commits:**
   ```bash
   bash fetch_october_commits.sh
   ```

2. **Get your commits:**
   ```bash
   bash fetch_october_commits.sh --author "your.email@usc.edu"
   ```

3. **Get commits in markdown:**
   ```bash
   bash fetch_october_commits.sh --format markdown
   ```

4. **Combine filters:**
   ```bash
   bash fetch_october_commits.sh --author "your.email@usc.edu" --format markdown
   ```

## Next Steps

When commits are made during October 24-31, 2025, simply run the script to retrieve the commit links!
